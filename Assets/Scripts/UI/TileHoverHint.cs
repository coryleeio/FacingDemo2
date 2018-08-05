using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gamepackage
{
    public class TileHoverHint : UIComponent
    {
        private GameObject prefab;
        private const int MaxLines = 20;
        private int LinesInUse = 0;
        private List<Text> lines = new List<Text>(MaxLines);

        public override void Hide()
        {
            GetComponent<TileHoverHint>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            GetComponent<TileHoverHint>().gameObject.SetActive(true);
        }

        public override void Refresh() { }

        public void ShowFor(Point location)
        {
            LinesInUse = 0;

            if (prefab == null)
            {
                prefab = Resources.Load<GameObject>("UI/TileHoverText");
                for (var i = 0; i < MaxLines; i++)
                {
                    var lineGameObject = GameObject.Instantiate(prefab);
                    lineGameObject.transform.SetParent(this.gameObject.transform, false);
                    lines.Add(lineGameObject.GetComponent<Text>());
                }
            }
            foreach (var line in lines)
            {
                line.gameObject.SetActive(false);
            }
            var level = Context.GameStateManager.Game.CurrentLevel;

            if (level.BoundingBox.Contains(location))
            {
                var entitiesInPosition = level.Grid[location].EntitiesInPosition;
                foreach(var entity in entitiesInPosition)
                {
                    AddLine(entity.Name);
                }
                AddLine(location.ToString());
                Show();
            }
            else
            {
                AddLine(location.ToString());
            }
        }

        private void AddLine(string lineText)
        {
            var text = lines[LinesInUse];
            text.text = lineText;
            LinesInUse++;
            text.gameObject.SetActive(true);
        }
    }
}
