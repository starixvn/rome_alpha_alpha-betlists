namespace Alpha.BetLists.Models
{
    public class NapUserFunctionPermission
    {
        public NapUserFunctionPermission()
        {
        }

        public NapUserFunctionPermission(int groupId, string fileNames, string funcName, byte actualPolicy)
        {
            GroupId = groupId;
            FileNames = fileNames;
            FuncName = funcName;
            ActualPolicy = actualPolicy;
        }

        public int GroupId { get; set; }

        public string FileNames { get; set; }

        public string FuncName { get; set; }

        public byte ActualPolicy { get; set; }
    }
}