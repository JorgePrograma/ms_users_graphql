namespace msusersgraphql.Models.Dtos;
public class PokemonDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public List<PokemonTypeDto> Types { get; set; }
    public PokemonSpritesDto Sprites { get; set; }
    public List<PokemonStatDto> Stats { get; set; }
    public List<PokemonAbilityDto> Abilities { get; set; }
}

public class PokemonTypeDto
{
    public string Name { get; set; }
}

public class PokemonSpritesDto
{
    public string FrontDefault { get; set; }
    public string FrontShiny { get; set; }
}

public class PokemonStatDto
{
    public string Name { get; set; }
    public int BaseStat { get; set; }
}

public class PokemonAbilityDto
{
    public string Name { get; set; }
    public bool IsHidden { get; set; }
}

public class PokemonListDto
{
    public int Count { get; set; }
    public string Next { get; set; }
    public string Previous { get; set; }
    public List<PokemonListItemDto> Results { get; set; }
}

public class PokemonListItemDto
{
    public string Name { get; set; }
    public string Url { get; set; }
}