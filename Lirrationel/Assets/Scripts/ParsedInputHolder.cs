using Assets.Scripts.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class ParsedInputHolder
    {
        public IList<StateElement> States { get; set; }

        public IList<DecorativeElement> Decoratives { get; set; }

        public IList<ObjectElement> Objects { get; set; }
    }
}
