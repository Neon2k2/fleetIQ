using System;
using AuthenticationService.Domain.ValueObjects;

namespace AuthenticationService.Domain.Entities;

    public class Company
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string OwnerId { get; private set; }
        public Address Address { get; private set; }
        public bool IsActive { get; private set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private readonly List<ApplicationUser> _users = new();
        public IReadOnlyCollection<ApplicationUser> Users => _users.AsReadOnly();

        private Company() { } // For EF Core

        public static Company Create(string name, ApplicationUser owner, Address address)
        {
            var company = new Company
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                OwnerId = owner.Id,
                Address = address
            };

            owner.CompanyId = company.Id;
            company._users.Add(owner);

            return company;
        }

        public void AddUser(ApplicationUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (_users.Any(u => u.Id == user.Id)) return;

            user.CompanyId = Id;
            _users.Add(user);
        }

        public void RemoveUser(ApplicationUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.Id == OwnerId) throw new InvalidOperationException("Cannot remove company owner");

            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.CompanyId = null;
                _users.Remove(existingUser);
            }
        }

        public void Update(string name, Address address)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public void Deactivate()
        {
            IsActive = false;
            foreach (var user in _users)
            {
                user.IsActive = false;
            }
        }
    }
