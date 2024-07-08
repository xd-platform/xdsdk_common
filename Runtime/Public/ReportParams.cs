using System.Collections.Generic;

namespace XD.SDK.Report
{
    public class ReportParams
    {
        public ReporterInfo Reporter { get; } // 举报人
        public ReporteeInfo Reportee { get; } // 被举报人
        public List<ReasonInfo> Reasons { get; } // 举报原因
        public string UserDescription { get; } // 玩家描述
        public List<string> EvidenceList { get; } // 玩家添加的附件
        public Dictionary<string, object> Extras { get; } // 附加信息补充玩家信息，透传给使用方
        

        private ReportParams(ReporterInfo reporter, ReporteeInfo reportee, List<ReasonInfo> reasons, string userDescription, List<string> evidenceList, Dictionary<string, object> extras)
        {
            Reporter = reporter;
            Reportee = reportee;
            Reasons = reasons;
            UserDescription = userDescription;
            EvidenceList = evidenceList;
            Extras = extras;
        }

        public class ReporterInfo
        {
            public Dictionary<string, object> Extras { get; }

            public ReporterInfo(Dictionary<string, object> extras)
            {
                Extras = extras;
            }

            public override string ToString()
            {
                return $"ReporterInfo: Extras={string.Join(",", Extras)}";
            }
        }

        public class ReporteeInfo
        {
            public string XdId { get; }
            public Dictionary<string, object> Extras { get; }

            public ReporteeInfo(string xdId, Dictionary<string, object> extras)
            {
                XdId = xdId;
                Extras = extras;
            }

            public override string ToString()
            {
                return $"ReporteeInfo: XdId={XdId},  Extras={string.Join(",", Extras)}";
            }
        }

        public class ReasonInfo
        {
            public long ID { get; } // 举报原因 ID，后台配置
            public string Title { get; } // 原因名称
            public Dictionary<string, object> Extras { get; } // 附加信息补充玩家信息，透传给使用方

            public ReasonInfo(long id, string title, Dictionary<string, object> extras)
            {
                ID = id;
                Title = title;
                Extras = extras;
            }

            public override string ToString()
            {
                return $"ReasonInfo: ID={ID}, Title={Title}, Extras={string.Join(",", Extras)}";
            }
        }

        public class Builder
        {
            private ReporterInfo _reporter;
            private ReporteeInfo _reportee;
            private List<ReasonInfo> _reasons;
            private string _userDescription;
            private List<string> _evidenceList;
            private Dictionary<string, object> _extras;
            public Builder SetReporter(ReporterInfo reporter)
            {
                _reporter = reporter;
                return this;
            }

            public Builder SetReportee(ReporteeInfo reportee)
            {
                _reportee = reportee;
                return this;
            }

            public Builder SetReasons(List<ReasonInfo> reasons)
            {
                _reasons = reasons;
                return this;
            }

            public Builder SetUserDescription(string userDescription)
            {
                _userDescription = userDescription;
                return this;
            }

            public Builder SetEvidenceList(List<string> evidenceList)
            {
                _evidenceList = evidenceList;
                return this;
            }

            public Builder SetExtras(Dictionary<string, object> extras)
            {
                _extras = extras;
                return this;
            }

            public ReportParams Build()
            {
                return new ReportParams(_reporter, _reportee, _reasons, _userDescription, _evidenceList, _extras);
            }
        }

        public override string ToString()
        {
            return $"ReportParams: {Reporter}, {Reportee}, {Reasons}, UserDescription={UserDescription}, EvidenceList={string.Join(",", EvidenceList)}, Extras={string.Join(",", Extras)}";
        }
    }
}