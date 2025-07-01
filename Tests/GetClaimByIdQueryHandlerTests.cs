using Application.Queries;
using Application.Queries.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class GetClaimByIdQueryHandlerTests
    {
        private readonly Mock<IClaimRepository> _mockClaimRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetClaimByIdQueryHandler _handler;

        public GetClaimByIdQueryHandlerTests()
        {
            _mockClaimRepository = new Mock<IClaimRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetClaimByIdQueryHandler(_mockClaimRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WithExistingId_ShouldReturnMappedClaim()
        {
            
            int claimId = 1;
            var query = new GetClaimByIdQuery(claimId);

            var vehicle = new Vehicle(1, 50000m, "Toyota", "Corolla");
            var person = new Person(1, "João da Silva", "12345678901", new DateTime(1980, 1, 1));
            var claim = new Claim(person.Id.Value, vehicle.Id.Value, 675.94m, claimId)
            {
                Vehicle = vehicle,
                Insured = person,
                CreateDate = DateTime.Now.AddDays(-5)
            };

            var expectedResponse = new GetClaimResponse
            {
                Id = claimId,
                ClaimValue = 675.94m,
                InsuredId = person.Id.Value,
                Insured = person,
                VehicleId = vehicle.Id.Value,
                Vehicle = vehicle,
                CreateDate = claim.CreateDate
            };

            _mockClaimRepository.Setup(r => r.GetById(claimId))
                .ReturnsAsync(claim);
            _mockMapper.Setup(m => m.Map<GetClaimResponse>(claim))
                .Returns(expectedResponse);

            
            var result = await _handler.Handle(query, CancellationToken.None);

            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);
            _mockClaimRepository.Verify(r => r.GetById(claimId), Times.Once);
            _mockMapper.Verify(m => m.Map<GetClaimResponse>(claim), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistingId_ShouldReturnNull()
        {
            
            int claimId = 999;
            var query = new GetClaimByIdQuery(claimId);

            _mockClaimRepository.Setup(r => r.GetById(claimId))
                .ReturnsAsync((Claim)null);

            
            var result = await _handler.Handle(query, CancellationToken.None);
           
            result.Should().BeNull();
            _mockClaimRepository.Verify(r => r.GetById(claimId), Times.Once);
        }
    }
} 