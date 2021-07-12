﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.DataAccess.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TagTypeEnum
    {
        None,

        Document,

        Paragraph,

        Graph
    }
}