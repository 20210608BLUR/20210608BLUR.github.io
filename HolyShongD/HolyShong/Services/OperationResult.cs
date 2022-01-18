using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//todo 迷路了
namespace HolyShong.Services
{
    public class OperationResult
    {
        public bool IsSuccessful { get; set; }
        public String Exception { get; set; }
        public Object Result { get; set; }
    }
}