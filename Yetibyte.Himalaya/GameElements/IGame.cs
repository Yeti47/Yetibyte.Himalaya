using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.Himalaya.Controls;

namespace Yetibyte.Himalaya.GameElements {

    public interface IGame {
        
        Scene CurrentScene { get; set; }

    }

}
