using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes
{
    public class TaskConfirmRecordBindingModel
    {
        public string ConfirmedMessage { get; set; }

        public PushStatus? PushStatus { get; set; }

        public PermanentCntrStatus? PermanentCntrStatus { get; set; }

        public bool? IsExcessive { get; set; }
    }
}
