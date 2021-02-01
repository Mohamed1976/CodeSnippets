const container = document.getElementById("container");
const observer = window.lozad();
observer.observe();
const fetchCount = 200;

const getPokemons = async ()  => {
    for (let i = 1; i <= fetchCount; i++) {
        const url = `https://pokeapi.co/api/v2/pokemon/${i}`;
        const data = await fetch(url);
        const pokemon =  await data.json();
        createPokemonView(pokemon);
        observer.observe();
    }
}

const createPokemonView = (pokemon) => {
    const name = pokemon.name;
    const types = pokemon.types.map(type => type.type.name);
    const abilities = pokemon.abilities.map(ability => ability.ability.name);

    console.log(`name[${name}], types[${types}], abilities[${abilities}]`);

    const pokemonElement = document.createElement('div');
    pokemonElement.classList.add('lozad');
    pokemonElement.classList.add('pokemon');

    const pokeInnerHTML = `
        <div class="img-container">
            <img class="lozad" data-src="https://pokeres.bastionbot.org/images/pokemon/${
							pokemon.id
						}.png" alt="${name}" />
        </div>
        <div class="info">
            <span class="number">#${pokemon.id
							.toString()
							.padStart(3, '0')}</span>
            <h3 class="name">${name}</h3>
            <small class="type">Type: <span>${types}</span></small>
            <small class="type">Ability: <span>${abilities}</span></small>

        </div>
    `;

	pokemonElement.innerHTML = pokeInnerHTML;
	container.appendChild(pokemonElement);    
}

//Entry point
(function () {
    console.log("Entry point");
    getPokemons();
})();