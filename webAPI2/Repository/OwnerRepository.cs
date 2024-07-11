using webAPI2.Data;
using webAPI2.Interface;
using webAPI2.Models;

namespace webAPI2.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(o=> o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfPokemon(int pokeId)
        {
            return _context.PokemonOwners.Where(pk => pk.PokemonId == pokeId).Select(o => o.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(pk => pk.OwnerId == ownerId).Select(p=> p.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(o=>o.Id == ownerId);
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }
    }
}
