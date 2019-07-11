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

namespace Microsoft.Azure.Commands.DataShare.Share
{  
    using System;
    using System.Management.Automation;
    using System.Globalization;
    using Microsoft.Azure.Commands.DataShare.Helpers;
    using Microsoft.Azure.Management.DataShare;
    using Microsoft.Azure.Commands.DataShare.Common;
    using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
    using Microsoft.Azure.PowerShell.Cmdlets.DataShare.Models;
    using Microsoft.Azure.PowerShell.Cmdlets.DataShare.Properties;
    using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;

    /// <summary>
    /// Defines Remove-AzDataShare cmdlet.
    /// </summary>
    [Cmdlet(
         "Remove",
         ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "DataShare",
         DefaultParameterSetName = ParameterSetNames.FieldsParameterSet,
         SupportsShouldProcess = true), OutputType(typeof(bool))]
    public class RemoveAzDataShare : AzureDataShareCmdletBase
    {
        /// <summary>
        /// The resource group name of the azure data share account.
        /// </summary>
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSetNames.FieldsParameterSet,
            HelpMessage = "The resource group name of the azure data share account")]
        [ResourceGroupCompleter()]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        /// <summary>
        /// Name of azure data share account.
        /// </summary>
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSetNames.FieldsParameterSet,
            HelpMessage = "Azure data share account name")]
        [ValidateNotNullOrEmpty]
        public string AccountName { get; set; }

        /// <summary>
        /// Name of azure data share.
        /// </summary>
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSetNames.FieldsParameterSet,
            HelpMessage = "Azure data share name")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// The resourceId of the azure data share.
        /// </summary>
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The resource id of the Azure data share",
            ParameterSetName = ParameterSetNames.ResourceIdParameterSet)]
        [ResourceGroupCompleter()]
        [ValidateNotNullOrEmpty]
        public string ResourceId { get; set; }

        /// <summary>
        /// Data share object.
        /// </summary>
        [Parameter(
            Mandatory = true,
            ParameterSetName = ParameterSetNames.ObjectParameterSet,
            ValueFromPipeline = true,
            HelpMessage = "Azure data share object")]
        [ValidateNotNullOrEmpty]
        public PSShare Share { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Return object (if specified).")]
        public SwitchParameter PassThru { get; set; }

        [Parameter()]
        public SwitchParameter Force { get; set; }

        [Parameter]
        public SwitchParameter AsJob { get; set; }

        public override void ExecuteCmdlet()
        {
            if (this.ShouldProcess(this.Name, VerbsCommon.Remove))
            {
                string resourceId = null;

                if (this.ParameterSetName.Equals(ParameterSetNames.ResourceIdParameterSet, StringComparison.OrdinalIgnoreCase))
                {
                    resourceId = this.ResourceId;
                }

                if (this.ParameterSetName.Equals(ParameterSetNames.ObjectParameterSet, StringComparison.OrdinalIgnoreCase))
                {
                    if (this.Share == null)
                    {
                        throw new PSArgumentNullException(
                            string.Format(CultureInfo.InvariantCulture, Resources.DataShareArgumentInvalid));
                    }

                    resourceId = this.Share.Id;
                }

                if (!string.IsNullOrEmpty(resourceId))
                {
                    var parseResourceId = new ResourceIdentifier(resourceId);
                    this.ResourceGroupName = parseResourceId.ResourceGroupName;
                    this.AccountName = parseResourceId.GetAccountName();
                    this.Name = parseResourceId.GetShareName();
                }

                if (this.AsJob)
                {
                    this.ConfirmAction(
                        this.Force,
                        this.Name,
                        this.MyInvocation.InvocationName,
                        this.Name,
                        () => this.DataShareManagementClient.Shares.BeginDelete(
                            this.ResourceGroupName,
                            this.AccountName,
                            this.Name));
                }
                else
                {
                    this.ConfirmAction(
                        this.Force,
                        this.Name,
                        this.MyInvocation.InvocationName,
                        this.Name,
                        () => this.DataShareManagementClient.Shares.Delete(
                            this.ResourceGroupName,
                            this.AccountName,
                            this.Name));
                }
            }

            if (this.PassThru)
            {
                this.WriteObject(true);
            }
        }
    }
}
