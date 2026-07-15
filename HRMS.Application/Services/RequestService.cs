using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HRMS.domain.Entities;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Services;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.DTOs.Request;

namespace HRMS.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly IMapper _mapper;
        private readonly IRequestRepository _requestRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public RequestService(IMapper mapper, IRequestRepository requestRepository, IEmployeeRepository employeeRepository)
        {
            _mapper = mapper;
            _requestRepository = requestRepository;
            _employeeRepository = employeeRepository;
        }
        public async Task<RequestResponseDto?> GetByIdAsync(int id) 
        {
            var request = await _requestRepository.GetByIdAsync(id);
            return request is null ? null : _mapper.Map<RequestResponseDto?>(request);
        }
        public async Task<IEnumerable<RequestResponseDto>> GetAllAsync()
        {
            var Requests = await _requestRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RequestResponseDto>>(Requests);
        }
        public async Task<IEnumerable<RequestResponseDto>?> GetByEmployeeIdAsync(int id)
        {
            var requests = await _requestRepository.GetByEmployeeIdAsync(id);
            return _mapper.Map<IEnumerable<RequestResponseDto>?>(requests);
        }
        public async Task<IEnumerable<RequestResponseDto>?> GetWithStatusAsync(int status)
        {
            var requests = await _requestRepository.GetWithStatusAsync(status);
            return _mapper.Map<IEnumerable<RequestResponseDto>>(requests);
        }
    }
}
