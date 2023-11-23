namespace Alpha.BetLists.DataAccesses
{
    using System;
    using System.Linq;
    using AlphaPlus.SmartPunter.Criteria;
    using AlphaPlus.SmartPunter.DataAccesses;
    using AlphaPlus.SmartPunter.Models;
    using Criteria.SmartPunter;
    using Fanex.Data.Repository;
    using Models;

    public class SmartPunterDataAccess : ISmartPunterDataAccess
    {
        private readonly IDynamicRepository dynamicRepository;

        public SmartPunterDataAccess(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public bool CheckInternalAdmin(InternalAdminChecker commandData)
        {
            var command = new InternalAdminCheckerCommand(commandData.CustId);

            dynamicRepository.Execute(command);

            return command.IsInternalAdmin;
        }

        public bool CheckPermission(NAPUserPermission criteriaData, string functionName)
        {
            var criteria = new NAPUserPermissionCriteria(criteriaData.CustId);

            var permissions = dynamicRepository.Fetch<NapUserFunctionPermission>(criteria);

            return permissions.Any(p => p.FuncName.Equals(functionName, StringComparison.OrdinalIgnoreCase));
        }

        public SMACustomer CheckSmartPunter(SmartPunterSMAChecker commandData)
        {
            var command = new SmartPunterSMACheckerCommand(commandData.CustId, commandData.Username);

            dynamicRepository.Execute(command);

            return new SMACustomer(command.IsSmartPunter, command.HasDownlineSmartPunter);
        }

        public void CheckSmartPunterAndWriteLog(SmartPunterSMACheckerWriteLog commandData)
        {
            var command = new SmartPunterSMACheckerWriteLogCommand(
                commandData.ListUserName,
                commandData.ActionId,
                commandData.IsRestricted,
                commandData.UserId,
                commandData.IP);

            dynamicRepository.Execute(command);
        }

        public void WriteLog(SmartPunterWriteLog commandData)
        {
            var command = new SmartPunterWriteLogCommand(
                commandData.ListUserName,
                commandData.ActionId,
                commandData.IsRestricted,
                commandData.UserId,
                commandData.IP);

            dynamicRepository.Execute(command);
        }

        public void WriteLogNoData(SmartPunterWriteLogNoData commandData)
        {
            var command = new SmartPunterWriteLogNoDataCommand(
                commandData.CustId,
                commandData.ActionId,
                commandData.IsRestricted,
                commandData.UserId,
                commandData.IP);

            dynamicRepository.Execute(command);
        }
    }
}