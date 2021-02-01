// grab DOM
const search = document.getElementById('search');
const matchList = document.getElementById('match-list');

// Execute a function when the user releases a key on the keyboard
search.addEventListener("keyup", function(event) {
    if (event.code === "Enter") {
        searchState(search.value);
    }
});

// search and filter data from state.json
const searchState = async (searchText) => {
    // using fetch api
    // console.log(searchText);
    // const res = await fetch('data/countries.json');
    // const states = await res.json();
    // //console.log(states);

    fetch('data/countries.json')
        .then((response) => {
            //console.log(response);
            return response.json();
        })
        .then((json) => {
            //console.log(json);
            //console.log(JSON.stringify(json));
            //Loop through objects
            // for(let i = 0; i < json.length; i++)
            // {
            //     console.log(`${i}, ${json[i].name}, ${json[i].code}`);
            // }

            let obj = json.find((x) => x.code === searchText);
            if(obj === undefined) {
                obj = json.find((x) => x.name === searchText);
            } 

            if(obj !== undefined) {
                console.log(`Obj found: ${obj.name}, ${obj.code}`);
                outputHtml(obj);
            }
        })
        .catch(error => {
            console.log(error);
        });    
};

// show results in HTML
const outputHtml = (match) => {

    //console.log(match);
    const found = `<div class="card card-body mb-1">
        <h4>
            ${match.name} (${match.code})
        </h4>
    </div>`;    
    
    matchList.innerHTML = found;
};