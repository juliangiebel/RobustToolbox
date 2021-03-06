﻿using JetBrains.Annotations;
using Robust.Server.Interfaces.Console;
using Robust.Server.Interfaces.Player;
using Robust.Shared.GameObjects;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.IoC;

namespace Robust.Server.Console.Commands
{
    [UsedImplicitly]
    internal sealed class AddCompCommand : IClientCommand
    {
        public string Command => "addcomp";
        public string Description => "Adds a component to an entity";
        public string Help => "addcomp <uid> <componentName>";

        public void Execute(IConsoleShell shell, IPlayerSession? player, string[] args)
        {
            if (args.Length != 2)
            {
                shell.SendText(player, "Wrong number of arguments");
                return;
            }

            var entityUid = EntityUid.Parse(args[0]);
            var componentName = args[1];

            var compManager = IoCManager.Resolve<IComponentManager>();
            var compFactory = IoCManager.Resolve<IComponentFactory>();
            var entityManager = IoCManager.Resolve<IEntityManager>();

            var entity = entityManager.GetEntity(entityUid);
            var component = (Component) compFactory.GetComponent(componentName);

            component.Owner = entity;

            compManager.AddComponent(entity, component);
        }
    }

    [UsedImplicitly]
    internal sealed class RemoveCompCommand : IClientCommand
    {
        public string Command => "rmcomp";
        public string Description => "Removes a component from an entity.";
        public string Help => "rmcomp <uid> <componentName>";
        public void Execute(IConsoleShell shell, IPlayerSession? player, string[] args)
        {
            if (args.Length != 2)
            {
                shell.SendText(player, "Wrong number of arguments");
                return;
            }

            var entityUid = EntityUid.Parse(args[0]);
            var componentName = args[1];

            var compManager = IoCManager.Resolve<IComponentManager>();
            var compFactory = IoCManager.Resolve<IComponentFactory>();

            var registration = compFactory.GetRegistration(componentName);

            compManager.RemoveComponent(entityUid, registration.Type);
        }
    }
}
