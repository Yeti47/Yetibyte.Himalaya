using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = System.String;

namespace Yetibyte.Himalaya.Content.Pipeline.SpriteAnimation {
    
    [ContentImporter(".anim", DisplayName = "Sprite Animation", DefaultProcessor = "SpriteAnimationProcessor")]
    public class SpriteAnimationImporter : ContentImporter<TInput> {

        public override TInput Import(string filename, ContentImporterContext context) {

            return File.ReadAllText(filename);

        }

    }

}
