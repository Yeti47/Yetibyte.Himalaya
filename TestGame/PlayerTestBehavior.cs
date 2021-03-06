﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.Collision;
using Yetibyte.Himalaya.GameElements;
using Yetibyte.Himalaya.Controls;
using System.Diagnostics;
using Yetibyte.Himalaya.Extensions;
using Yetibyte.Himalaya;
using Yetibyte.Himalaya.Debugging;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame {

    public class PlayerTestBehavior : Behavior, IDraw {

        private ControlListener _controlListener;
        private CollisionController _collisionController;

        public float Speed { get; set; } = 150f;

        public int DrawOrder { get; set; }

        public override void Awake() {

            _controlListener = GameEntity.GetComponent<ControlListener>();
            _collisionController = GameEntity.GetComponent<CollisionController>();
            GameEntity.Scene.Camera.Target = GameEntity;

        }

        public override void Update(GameTime gameTime, float globalTimeScale) {

            float deltaTime = gameTime.DeltaTime();

            _collisionController.Velocity = new Vector2(_controlListener.GetAxisValue("Horizontal") * Speed * deltaTime, _controlListener.GetAxisValue("Vertical") * Speed * deltaTime);

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {

            DebugUtility.VisualizeColliders(spriteBatch, GameEntity.Scene, Color.Red);

            LineSegment lineA = new LineSegment(new Vector2(30, -200), new Vector2(30, -100));
            LineSegment lineB = new LineSegment(new Vector2(10, -140), new Vector2(90, -120));

            DebugUtility.DrawLine(spriteBatch, lineA, Color.Green);
            DebugUtility.DrawLine(spriteBatch, lineB, Color.Orange);

            //bool intersect = LineSegment.Intersect(lineA, lineB, out Vector2 intersection);

            //Debug.WriteLine("LineA and LineB intersect: " + intersect + ". Intersection point: " + intersection);

            float angle = Vector2Helper.AngleBetween(lineA.Delta, lineB.Delta);

            Vector2[] polyPointsA = { new Vector2(-280, -60), new Vector2(-160, -60), new Vector2(-140, 10), new Vector2(-160, 50), new Vector2(-280, 50), new Vector2(-300, 10) };
            Vector2[] polyPointsB = polyPointsA.Reverse().ToArray();

            Vector2[] polyPointsC = { new Vector2(-280, -150), new Vector2(-300, -180), new Vector2(-280, -240), new Vector2(-140, -240), new Vector2(-160, -180), new Vector2(-140, -150) };

            Polygon polygonA = new Polygon(polyPointsA);
            Polygon polygonB = new Polygon(polyPointsB);
            polygonB = Polygon.Translate(polygonB, Vector2.UnitY * 150f);
            Polygon polygonC = new Polygon(polyPointsC);
            polygonC = polygonC.Translate(Vector2.UnitY * 20f);
            Polygon polygonD = polygonC.Reverse().Translate(Vector2.UnitX * 450f);

            RectangleF rect = new RectangleF(-380, -80, 30, 30);
            Polygon polygonE = new Polygon(rect);
            Polygon polygonF = new Polygon(rect);

            DebugUtility.DrawEdges(spriteBatch, polygonA, Color.Lavender);
            DebugUtility.DrawEdges(spriteBatch, polygonB, Color.Indigo);
            DebugUtility.DrawEdges(spriteBatch, polygonC, Color.Indigo);
            DebugUtility.DrawEdges(spriteBatch, polygonD, Color.Indigo);
            DebugUtility.DrawEdges(spriteBatch, polygonE, Color.DarkOrchid);
            /*
            Debug.WriteLine("polygonA in convex: " + polygonA.IsConvex);
            Debug.WriteLine("polygonB in convex: " + polygonB.IsConvex);
            Debug.WriteLine("polygonC in convex: " + polygonC.IsConvex);
            Debug.WriteLine("polygonD in convex: " + polygonD.IsConvex);
            Debug.WriteLine("polygonE in convex: " + polygonE.IsConvex);
            Debug.WriteLine("polygonA is clockwise: " + polygonA.IsClockwise);
            Debug.WriteLine("polygonB is clockwise: " + polygonB.IsClockwise);
            Debug.WriteLine("polygonC is clockwise: " + polygonC.IsClockwise);
            Debug.WriteLine("polygonD is clockwise: " + polygonD.IsClockwise);
            Debug.WriteLine("polygonE is clockwise: " + polygonE.IsClockwise);

            Debug.WriteLine("polygonD is rectangular: " + polygonD.IsRectangle);
            Debug.WriteLine("polygonE is rectangular: " + polygonE.IsRectangle);

            Debug.WriteLine("PolygonE and PolygonF are equal: " + (polygonE == polygonF));
            Debug.WriteLine("PolygonA and PolygonB are equal: " + (polygonA == polygonB));

            Debug.WriteLine("PolygonA as string: " + polygonA);
            */

        }

        protected override void OnTrigger(Collider ownCollider, Collider otherCollider) {
            
        }

        protected override void OnTriggerEnter(Collider ownCollider, Collider otherCollider) {
            
        }

        protected override void OnTriggerLeave(Collider ownCollider, Collider otherCollider) {
            
        }
    }

}
