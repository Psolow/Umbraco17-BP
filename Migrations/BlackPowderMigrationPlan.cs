using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;

namespace UmbracoProject1.Migrations;

public class BlackPowderMigrationComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.AddNotificationHandler<UmbracoApplicationStartingNotification, RunBlackPowderMigration>();
    }
}

public class RunBlackPowderMigration : INotificationHandler<UmbracoApplicationStartingNotification>
{
    private readonly IMigrationPlanExecutor _migrationPlanExecutor;
    private readonly ICoreScopeProvider _scopeProvider;
    private readonly IKeyValueService _keyValueService;
    private readonly IRuntimeState _runtimeState;

    public RunBlackPowderMigration(
        IMigrationPlanExecutor migrationPlanExecutor,
        ICoreScopeProvider scopeProvider,
        IKeyValueService keyValueService,
        IRuntimeState runtimeState)
    {
        _migrationPlanExecutor = migrationPlanExecutor;
        _scopeProvider = scopeProvider;
        _keyValueService = keyValueService;
        _runtimeState = runtimeState;
    }

    public void Handle(UmbracoApplicationStartingNotification notification)
    {
        if (_runtimeState.Level < RuntimeLevel.Run)
        {
            return;
        }

        var plan = new BlackPowderMigrationPlan();
        var upgrader = new Upgrader(plan);
        upgrader.Execute(_migrationPlanExecutor, _scopeProvider, _keyValueService);
    }
}

public class BlackPowderMigrationPlan : MigrationPlan
{
    public BlackPowderMigrationPlan() : base("BlackPowder")
    {
        From(string.Empty)
            .To<CreateBlackPowderDocumentTypesMigration>("black-powder-1.0.0")
            .To<UpdateBlackPowderDocumentTypesMigration>("black-powder-2.0.0")
            .To<UpdateUnitDataModelMigration>("black-powder-3.0.0");
    }
}
