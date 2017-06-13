namespace Alpha.BetLists.Models
{
    public class NapUserFunctionPermission
    {
        public NapUserFunctionPermission(int groupId, string fileNames, string funcName, byte actualPolicy)
        {
            GroupId = groupId;
            FileNames = fileNames;
            FuncName = funcName;
            ActualPolicy = actualPolicy;
        }

        public int GroupId { get; }

        public string FileNames { get; }

        public string FuncName { get; }

        public byte ActualPolicy { get; }
    }
}