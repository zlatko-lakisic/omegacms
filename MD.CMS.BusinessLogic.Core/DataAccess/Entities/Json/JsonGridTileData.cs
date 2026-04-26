using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Json
{
    public class JsonGridTileData
    {
        public enum GridTileLayout: int
        {
            Row = 1,
            Column = 2
        }

        #region Attributes
        private int _width;
        private int _width_medium;
        private int _width_small;
        private int _height;
        private int _height_medium;
        private int _height_small;
        #endregion

        public int width
        {
            get
            {
                if (_width.Equals(default(int)))
                {
                    _width = 30;
                }
                return _width;
            }
            set
            {
                _width = value;
            }
        }
        public int width_medium
        {
            get
            {
                if (_width_medium.Equals(default(int)))
                {
                    _width_medium = 30;
                }
                return _width_medium;
            }
            set
            {
                _width_medium = value;
            }
        }
        public int width_small
        {
            get
            {
                if (_width.Equals(default(int)))
                {
                    _width_small = 30;
                }
                return _width_small;
            }
            set
            {
                _width_small = value;
            }
        }
        public int height
        {
            get
            {
                if (_height.Equals(default(int)))
                {
                    _height = 150;
                }
                return _height;
            }
            set
            {
                _height = value;
            }
        }
        public int height_medium
        {
            get
            {
                if (_height_medium.Equals(default(int)))
                {
                    _height_medium = 150;
                }
                return _height_medium;
            }
            set
            {
                _height_medium = value;
            }
        }
        public int height_small
        {
            get
            {
                if (_height_small.Equals(default(int)))
                {
                    _height_small = 150;
                }
                return _height_small;
            }
            set
            {
                _height_small = value;
            }
        }
        public string id { get; set; }
        public string uniqueId { get; set; }
        public string parentId { get; set; }
        public int index { get; set; }
        public GridTileLayout layout { get; set; }
        public int whiteframe { get; set; }
        public bool layoutPadding { get; set; }
        public bool layoutMargin { get; set; }
        public bool layoutWrap { get; set; }
    }
}
