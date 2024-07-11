using webAPI2.Models;

namespace webAPI2.Interface
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int id);
        Pokemon GetPokemonName(string name);
        bool PokemonExists(int pokeId);
        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);
        bool Save();
    }
}
