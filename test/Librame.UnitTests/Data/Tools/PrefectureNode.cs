#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.UnitTests.Data.Tools
{
    public class PrefectureNode
    {
        public PrefectureNode()
        {
            Id = ParentId = 0;
        }

        public int Id;

        public int ParentId;

        public string CodeId;

        /// <summary>
        /// 首字母
        /// </summary>
        //[Newtonsoft.Json.JsonIgnore]
        public string Initial;

        //[Newtonsoft.Json.JsonIgnore]
        public string Acronym;

        /// <summary>
        /// 拼音
        /// </summary>
        public string Name;

        /// <summary>
        /// 标题
        /// </summary>
        //[Newtonsoft.Json.JsonIgnore]
        public string Title;

        [Newtonsoft.Json.JsonIgnore]
        public int Spaces;

        public List<PrefectureNode> Childs;
    }
}
