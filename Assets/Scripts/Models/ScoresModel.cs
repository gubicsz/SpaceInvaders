using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace SpaceInvaders
{
    [Serializable]
    public class ScoreItem
    {
        public int Score;
        public string Date;
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

        private Scoreboard _dummyScoreboad = new Scoreboard()
        {
            Items = new ScoreItem[]
            {
                new ScoreItem() { Score = 1230, Date = DateTime.Now.AddDays(-2).ToString("MM/dd/yyyy HH:mm") },
                new ScoreItem() { Score = 150, Date = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy HH:mm") },
                new ScoreItem() { Score = 90, Date = DateTime.Now.ToString("MM/dd/yyyy HH:mm") },
            }
        };

        public ScoresModel(StorageService storageService)
        {
            _storageService = storageService;
            Scoreboard = new ReactiveCollection<ScoreItem>();
        }

        public void Add(ScoreItem scoreItem)
        {
            // Add new item to the original list
            var originalList = Scoreboard.ToList();
            originalList.Add(scoreItem);

            // Order items and update scoreboard
            var orderedList = originalList.OrderByDescending(x => x.Score).Take(10);
            UpdateScoreboard(orderedList);
        }

        public void Save()
        {
            // Save scoreboard to the device
            _storageService.Save(_storageKey, new Scoreboard()
            {
                Items = Scoreboard.ToArray()
            });
        }

        public void Load()
        {
            // Load scoreboard from the device
            var scoreboard = _storageService.Load<Scoreboard>(_storageKey);

            // Load dummy scoreboard for testing purposes
            if (scoreboard == null)
            {
                scoreboard = _dummyScoreboad;
            }

            UpdateScoreboard(scoreboard.Items);
        }

        private void UpdateScoreboard(IEnumerable<ScoreItem> items)
        {
            // Clear reactive list
            Scoreboard.Clear();

            // Add items one by one
            foreach (var item in items)
            {
                Scoreboard.Add(item);
            }
        }
    }
}
