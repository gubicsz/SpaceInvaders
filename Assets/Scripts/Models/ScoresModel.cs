using System;
using System.Linq;
using UniRx;

namespace SpaceInvaders
{
    [Serializable]
    public class ScoreItem
    {
        public int Score;
        public DateTime Date;
    }

    [Serializable]
    public class Scoreboard
    {
        public ScoreItem[] Items;
    }

    public class ScoresModel
    {
        public ReactiveCollection<ScoreItem> Scoreboard { get; private set; }

        private const string _storageKey = "Scoreboard";
        private StorageService _storageService;

        public ScoresModel(StorageService storageService)
        {
            _storageService = storageService;
            Scoreboard = new ReactiveCollection<ScoreItem>();
        }

        public void Save()
        {
            var scoreboard = new Scoreboard()
            {
                Items = Scoreboard.ToArray()
            };

            _storageService.Save(_storageKey, scoreboard);
        }

        public void Load()
        {
            var scoreboard = _storageService.Load<Scoreboard>(_storageKey);

            Scoreboard.Clear();

            foreach (var item in scoreboard.Items)
            {
                Scoreboard.Add(item);
            }
        }
    }
}
