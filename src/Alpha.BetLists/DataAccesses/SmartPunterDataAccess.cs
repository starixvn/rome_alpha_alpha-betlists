namespace Alpha.BetLists.DataAccesses
{
    using System;
    using System.Linq;
    using Criteria.SmartPunter;
    using Fanex.Data.Repository;
    using SmartPunter.Criteria;
    using SmartPunter.DataAccesses;
    using SmartPunter.Models;

    public class SmartPunterDataAccess : ISmartPunterDataAccess
    {
        private readonly IDynamicRepository dynamicRepository;

        public SmartPunterDataAccess(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public bool CheckInternalAdmin(InternalAdminChecker commandData)
        {
            var command = new InternalAdminCheckerCommand(commandData);

            dynamicRepository.Execute(command);

            return command.IsInternalAdmin;
        }

        public bool CheckPermission(NAPUserPermission criteriaData, string functionName)
        {
            var criteria = new NAPUserPermissionCriteria(criteriaData);

            var functionNames = dynamicRepository.Fetch<string>(criteria);

            return functionNames.Any(name => name.Equals(functionName, StringComparison.OrdinalIgnoreCase));
        }

        public SMACustomer CheckSmartPunter(SmartPunterSMAChecker commandData)
        {
            var command = new SmartPunterSMACheckerCommand(commandData);

            dynamicRepository.Execute(command);

            return new SMACustomer(command.IsSmartPunter, command.HasDownlineSmartPunter);
        }

        public void CheckSmartPunterAndWriteLog(SmartPunterSMACheckerWriteLog commandData)
        {
            var command = new SmartPunterSMACheckerWriteLogCommand(commandData);

            dynamicRepository.Execute(command);
        }

        public void WriteLog(SmartPunterWriteLog commandData)
        {
            var command = new SmartPunterWriteLogCommand(commandData);

            dynamicRepository.Execute(command);
        }

        public void WriteLogNoData(SmartPunterWriteLogNoData commandData)
        {
            var command = new SmartPunterWriteLogNoDataCommand(commandData);

            dynamicRepository.Execute(command);
        }
    }
}