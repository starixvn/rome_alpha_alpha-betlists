namespace Alpha.BetLists.Builders.HtmlBuilders
{
    using System;
    using System.Collections.Generic;
    using AgentPTBuilder;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MasterPTBuilder;
    using SuperPTBuilder;

    /// <summary>
    /// Extend CoreBetList HTML Builder
    /// </summary>
    public static class BetListHtmlBuilderExt
    {
        private const string SUPERPT_BUILDER_NAMESPACE = "Alpha.BetLists.Builders.HtmlBuilders.SuperPTBuilder";
        private const string MASTERPT_BUILDER_NAMESPACE = "Alpha.BetLists.Builders.HtmlBuilders.MasterPTBuilder";
        private const string AGENTPT_BUILDER_NAMESPACE = "Alpha.BetLists.Builders.HtmlBuilders.AgentPTBuilder";

        public static string BuildSuperPT(this BetListHTMLBuilder factory, ITicket ticket, List<ITicketData> ticketData)
        {
            ISuperPT superPTBuilder = CreateSuperPTBuilder(ticket.BetTypeId);
            return superPTBuilder.Render(ticket, ticketData);
        }

        public static string BuildMasterPT(this BetListHTMLBuilder factory, ITicket ticket, List<ITicketData> ticketData)
        {
            IMasterPT masterPTBuilder = CreateMasterPTBuilder(ticket.BetTypeId);
            return masterPTBuilder.Render(ticket, ticketData);
        }

        public static string BuildAgentPT(this BetListHTMLBuilder factory, ITicket ticket, List<ITicketData> ticketData)
        {
            IAgentPT agentPTBuilder = CreateAgentPTBuilder(ticket.BetTypeId);
            return agentPTBuilder.Render(ticket, ticketData);
        }

        /// <summary>
        /// Creates the master PT builder.
        /// </summary>
        /// <returns></returns>
        private static IMasterPT CreateMasterPTBuilder(int bettype)
        {
            string typeName = string.Join(
                                            null,
                                            new string[]
                                            {
                                                MASTERPT_BUILDER_NAMESPACE,
                                                ".MasterPT",
                                                bettype.ToString()
                                            }
                                         );

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new MasterPT1();
            }
            else
            {
                return Activator.CreateInstance(type) as IMasterPT;
            }
        }

        /// <summary>
        /// Creates the super PT builder.
        /// </summary>
        /// <returns></returns>
        private static ISuperPT CreateSuperPTBuilder(int bettype)
        {
            string typeName = string.Join(
                                            null,
                                            new string[]
                                            {
                                                SUPERPT_BUILDER_NAMESPACE,
                                                ".SuperPT",
                                                bettype.ToString()
                                            }
                                         );

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new SuperPT1();
            }
            else
            {
                return Activator.CreateInstance(type) as ISuperPT;
            }
        }

        /// <summary>
        /// Creates the agent PT builder.
        /// </summary>
        /// <returns></returns>
        private static IAgentPT CreateAgentPTBuilder(int bettype)
        {
            string typeName = string.Join(
                                           null,
                                           new string[]
                                            {
                                                AGENTPT_BUILDER_NAMESPACE,
                                                ".AgentPT",
                                                bettype.ToString()
                                            }
                                        );

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new AgentPT1();
            }
            else
            {
                return Activator.CreateInstance(type) as IAgentPT;
            }
        }
    }
}