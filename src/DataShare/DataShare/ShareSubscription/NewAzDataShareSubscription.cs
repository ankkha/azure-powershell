﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.Azure.Commands.DataShare.ShareSubscription
{
    using System.Management.Automation;
    using Microsoft.Azure.Commands.DataShare.Common;
    using Microsoft.Azure.Commands.DataShare.Helpers;
    using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
    using Microsoft.Azure.Management.DataShare;
    using Microsoft.Azure.Management.DataShare.Models;
    using Microsoft.Azure.PowerShell.Cmdlets.DataShare.Models;

    /// <summary>
    /// Defines the New-DataShareSubscription cmdlet.
    /// </summary>
    [Cmdlet("New", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "DataShareSubscription", DefaultParameterSetName = ParameterSetNames.FieldsParameterSet, SupportsShouldProcess = true), OutputType(typeof(PSShare))]
    public class NewAzDataShareSubscription : AzureDataShareCmdletBase
    {
        /// <summary>
        /// The resource group name of the azure data share account.
        /// </summary>
        [Parameter(Mandatory = true, 
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSetNames.FieldsParameterSet,
            HelpMessage = "The resource group name of the azure data share account")]
        [ResourceGroupCompleter()]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        /// <summary>
        /// Name of azure data share account.
        /// </summary>
        [Parameter(Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSetNames.FieldsParameterSet,
            HelpMessage = "Azure data share account name")]
        [ValidateNotNullOrEmpty]
        public string AccountName { get; set; }

        /// <summary>
        /// Name of the azure data share subscription.
        /// </summary>
        [Parameter(Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSetNames.FieldsParameterSet, 
            HelpMessage = "Azure data share subscription name")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// InvitationId of invitation.
        /// </summary>
        [Parameter(Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSetNames.FieldsParameterSet, 
            HelpMessage = "Azure data share invitationId")]
        [ValidateNotNullOrEmpty]
        public string InvitationId { get; set; }

        public override void ExecuteCmdlet()
        {
            this.ConfirmAction(
                this.MyInvocation.InvocationName,
                this.Name,
                this.NewShareSubscription);
        }

        private void NewShareSubscription()
        {
            if (this.ShouldProcess(this.Name, VerbsCommon.New))
            {
                ShareSubscription shareSubscription = this.DataShareManagementClient.ShareSubscriptions.Create(
                    this.ResourceGroupName,
                    this.AccountName,
                    this.Name,
                    new ShareSubscription()
                    {
                        InvitationId = this.InvitationId
                    });

                this.WriteObject(shareSubscription.ToPsObject());
            }
        }
    }
}
