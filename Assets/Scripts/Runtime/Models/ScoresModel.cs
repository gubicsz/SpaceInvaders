using SpaceInvaders.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace SpaceInvaders.Models
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

        private readonly IStorageService _storageService;

        private const string _storageKey = "Scoreboard";

        public ScoresModel(IStorageService storageService)
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
            var orderedList = originalList
                .OrderByDescending(x => x.Score)
                .Take(10).ToList();

            // Update scoreboard
            if (orderedList != null)
            {
                UpdateScoreboard(orderedList);
            }
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

            // Update scoreboard
            if (scoreboard != null)
            {
                UpdateScoreboard(scoreboard.Items);
            }
        }

        private void UpdateScoreboard(IList<ScoreItem> items)
        {
            // Handle error
            if (items == null)
            {
                return;
            }

            // Clear reactive list
            Scoreboard.Clear();

            // Add items one by one
            for (int i = 0; i < items.Count; i++)
            {
                Scoreboard.Add(items[i]);
            }
        }
    }
}
