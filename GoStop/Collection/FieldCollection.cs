using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GoStop.Card;
using MG_Library;

namespace GoStop.Collection
{
    public class FieldCollection : CardCollection
    {
        private InputManager input;
        public FieldCollection() : base()
        { }

        public Task FindMatchTask(Hanafuda card)
        {
            List<Hanafuda> matches = new List<Hanafuda>();
            foreach (Hanafuda match in cards)
            {
                if (card.Month != match.Month)
                    continue;
                matches.Add(match);
            }
            if (matches.Count == 0)
                cards.Add(card);
            if (matches.Count == 3)
            {
                foreach (Hanafuda match in matches)
                {
                    cards.Remove(match);
                }
            }
            if (matches.Count > 1)
            {

                FieldEventArgs args = new FieldEventArgs();
                args.pairedCards = matches;
                OnMultipleMatchFound(args);
            }
            cards.Remove(matches[0]);
            matches.Add(card);
            return null;
        }

        protected virtual async Task<Hanafuda> AwaitSelectionTask(List<Hanafuda> matches)
        {
            var result = await Task.Run(() =>
            {

                return null;
            });
        }

        public event EventHandler<FieldEventArgs> MultipleMatchFound;
        public event EventHandler<FieldEventArgs> CardsPaired;

        protected virtual void OnMultipleMatchFound(FieldEventArgs args)
        {
            MultipleMatchFound?.Invoke(this, args);
        }

        protected virtual void OnCardsPaired(FieldEventArgs args)
        {
            CardsPaired?.Invoke(this, args);
        }
    }

    public class FieldEventArgs : EventArgs
    {
        public List<Hanafuda> pairedCards { get; set; }
    }
}
