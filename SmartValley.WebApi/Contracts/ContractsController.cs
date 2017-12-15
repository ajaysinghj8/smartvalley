﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application.Contracts;

namespace SmartValley.WebApi.Contracts
{
    [Route("api/contracts")]
    public class ContractsController : Controller
    {
        private readonly NethereumOptions _nethereumOptions;

        public ContractsController(NethereumOptions nethereumOptions)
        {
            _nethereumOptions = nethereumOptions;
        }

        [Route("projectManager")]
        public ContractResponse GetProjectManagerContract()
        {
            var projectManagerContractOptions = _nethereumOptions.ProjectManagerContract;
            return new ContractResponse
            {
                       Address = projectManagerContractOptions.Address,
                       Abi = projectManagerContractOptions.Abi,
                   };
        }

        [Route("project")]
        public ProjectContractResponse GetProjectContractAbi()
        {
            return new ProjectContractResponse
                   {
                       Abi = "[{\"constant\":true,\"inputs\":[],\"name\":\"svt\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"name\":\"\",\"type\":\"string\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"REQUIRED_SUBMISSIONS_COUNT\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"getEstimates\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256[]\"},{\"name\":\"\",\"type\":\"uint256[]\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"isScored\",\"outputs\":[{\"name\":\"\",\"type\":\"bool\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"submissionsCount\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"_expertiseArea\",\"type\":\"uint256\"},{\"name\":\"_questionIds\",\"type\":\"uint256[]\"},{\"name\":\"_scores\",\"type\":\"uint256[]\"},{\"name\":\"_commentHashes\",\"type\":\"bytes32[]\"}],\"name\":\"submitEstimates\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"estimates\",\"outputs\":[{\"name\":\"questionId\",\"type\":\"uint256\"},{\"name\":\"expertAddress\",\"type\":\"address\"},{\"name\":\"score\",\"type\":\"uint256\"},{\"name\":\"commentHash\",\"type\":\"bytes32\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"areaSubmissionsCounters\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"author\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"_owner\",\"type\":\"address\"}],\"name\":\"changeOwner\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[],\"name\":\"confirmOwner\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"newOwner\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"\",\"type\":\"uint256\"},{\"name\":\"\",\"type\":\"address\"}],\"name\":\"expertsByArea\",\"outputs\":[{\"name\":\"\",\"type\":\"bool\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"score\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"getEstimatesCount\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"name\":\"_author\",\"type\":\"address\"},{\"name\":\"_name\",\"type\":\"string\"},{\"name\":\"_svtAddress\",\"type\":\"address\"},{\"name\":\"_estimateRewardWEI\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"}]"
                   };
        }

        [Route("token")]
        public ContractResponse GetTokenContractAbi()
        {
            var tokenContractOptions = _nethereumOptions.TokenContract;
            return new ContractResponse
            {
                       Address = tokenContractOptions.Address,
                       Abi = tokenContractOptions.Abi,
                   };
        }

        [Route("minter")]
        public ContractResponse GetMinterContractAbi()
        {
            var tokenContractOptions = _nethereumOptions.MinterContract;
            return new ContractResponse
                   {
                       Address = tokenContractOptions.Address,
                       Abi = tokenContractOptions.Abi,
                   };
        }
    }
}