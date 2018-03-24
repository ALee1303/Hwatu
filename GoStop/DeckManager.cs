using Microsoft.Xna.Framework;

namespace GoStop
{
    enum CardState { Hidden, OnBoard, Taken}
    public class DeckManager
    {
        CardState[,] deckStatus;

        public DeckManager()
        {
            deckStatus = new CardState[12, 4];
            for (int i=0; i<11; i++)
            {
                for (int j = 0; j < 4; j++)
                    deckStatus[i, j] = CardState.Hidden;
            }
        }
    }
}
