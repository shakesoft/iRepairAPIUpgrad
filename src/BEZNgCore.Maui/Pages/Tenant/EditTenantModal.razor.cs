using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Components;
using BEZNgCore.Common;
using BEZNgCore.Editions.Dto;
using BEZNgCore.Maui.Core.Components;
using BEZNgCore.Maui.Core.Threading;
using BEZNgCore.Maui.Models.Tenants;
using BEZNgCore.MultiTenancy;
using BEZNgCore.MultiTenancy.Dto;

namespace BEZNgCore.Maui.Pages.Tenant;

public partial class EditTenantModal : BEZNgCoreMainLayoutPageComponentBase
{
    [Parameter] public int TenantId { get; set; }

    private readonly ITenantAppService _tenantAppService;

    private readonly ICommonLookupAppService _commonLookupAppService;

    private EditTenantModel EditTenantModel { get; set; } = new();

    public EditTenantModal()
    {
        _tenantAppService = Resolve<ITenantAppService>();
        _commonLookupAppService = Resolve<ICommonLookupAppService>();
    }

    protected override async Task OnInitializedAsync()
    {
        EditTenantModel = new EditTenantModel();

        await SetBusyAsync(async () =>
        {
            EditTenantModel =
                ObjectMapper.Map<EditTenantModel>(await _tenantAppService.GetTenantForEdit(new EntityDto(TenantId)));
            EditTenantModel.IsUnlimitedTimeSubscription = EditTenantModel.SubscriptionEndDateUtc == null;
            await PopulateEditionsCombobox();
        });

        await SetPageHeader(L("EditTenant"));
    }


    private async Task UpdateTenantAsync()
    {
        await SetBusyAsync(async () =>
        {
            await WebRequestExecuter.Execute(async () =>
            {
                EditTenantModel.NormalizeEditTenantInputModel();
                var input = ObjectMapper.Map<TenantEditDto>(EditTenantModel);

                await _tenantAppService.UpdateTenant(input);
            }, async () => { await UserDialogsService.AlertSuccess(L("SuccessfullySaved")); });
        });
    }

    private async Task PopulateEditionsCombobox()
    {
        var editions = await _commonLookupAppService.GetEditionsForCombobox();
        EditTenantModel.Editions = editions.Items.ToList();

        EditTenantModel.Editions.Insert(0, new SubscribableEditionComboboxItemDto(EditTenantModel.NotAssignedValue,
            $"- {L("NotAssigned")} -", null));

        EditTenantModel.SelectedEdition = EditTenantModel.EditionId?.ToString() ?? EditTenantModel.NotAssignedValue;
    }
}