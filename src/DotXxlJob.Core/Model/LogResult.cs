using System.Runtime.Serialization;

namespace com.xxl.job.core.biz.model
{
    public class LogResult
    {

        public LogResult(int fromLine ,int toLine,string content,bool isEnd)
        {
            this.fromLineNum = fromLine;
            this.toLineNum = toLine;
            this.logContent = content;
            this.isEnd = isEnd;
        }
        
        public int fromLineNum ;
        public int toLineNum ;
        public string logContent ;
        public bool isEnd ;
    }
}