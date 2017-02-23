namespace Alpha.BetLists.Builders.HtmlBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using AgentPTBuilder;
    using Fanex.BetList.Core.Entities;
    using MasterPTBuilder;
    using SuperPTBuilder;

    public static class BetListHtmlBuilderExt
    {
        private const string SuperPTBuilderNameSpace = "Alpha.BetLists.Builders.HtmlBuilders.SuperPTBuilder";
        private const string MasterPTBuilderNameSpace = "Alpha.BetLists.Builders.HtmlBuilders.MasterPTBuilder";
        private const string AgentPTBuilderNameSpace = "Alpha.BetLists.Builders.HtmlBuilders.AgentPTBuilder";

        public static string BuildSuperPT(ITicket ticket, List<ITicketData> ticketData)
        {
            ISuperPT superPTBuilder = CreateSuperPTBuilder(ticket.BetTypeId);
            return superPTBuilder.Render(ticket, ticketData);
        }

        public static string BuildMasterPT(ITicket ticket, List<ITicketData> ticketData)
        {
            IMasterPT masterPTBuilder = CreateMasterPTBuilder(ticket.BetTypeId);
            return masterPTBuilder.Render(ticket, ticketData);
        }

        public static string BuildAgentPT(ITicket ticket, List<ITicketData> ticketData)
        {
            IAgentPT agentPTBuilder = CreateAgentPTBuilder(ticket.BetTypeId);
            return agentPTBuilder.Render(ticket, ticketData);
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private static IMasterPT CreateMasterPTBuilder(int bettype)
        {
            string typeName = string.Join(
                                            null,
                                            new string[]
                                            {
                                                MasterPTBuilderNameSpace,
                                                ".MasterPT",
                                                bettype.ToString()
                                            });

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

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private static ISuperPT CreateSuperPTBuilder(int bettype)
        {
            string typeName = string.Join(
                                            null,
                                            new string[]
                                            {
                                                SuperPTBuilderNameSpace,
                                                ".SuperPT",
                                                bettype.ToString()
                                            });

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

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private static IAgentPT CreateAgentPTBuilder(int bettype)
        {
            string typeName = string.Join(
                                           null,
                                           new string[]
                                            {
                                                AgentPTBuilderNameSpace,
                                                ".AgentPT",
                                                bettype.ToString()
                                            });

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