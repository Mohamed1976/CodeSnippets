
const API = 'https://api.tvmaze.com';

//DOM Elements
const searchForm = document.getElementById('search-form');
const cards = document.getElementById('cards');
const resultContainer = document.getElementById('results');

window.addEventListener('hashchange', () => {
    const hash = window.location.hash; 

    if(hash !== "") {
        getSeriesDetails(hash);
    }    
});

document.addEventListener('DOMContentLoaded', e => {
    const hash = window.location.hash; 

    if(hash !== "") {
        getSeriesDetails(hash);
    }
})

const  getSeriesDetails = async (seriesId) => {
    
    cards.style.display = "none";
    resultContainer.innerHTML = '';

    const singleElement = document.querySelector('.single'); 
    if(singleElement !== null) {
        singleElement.parentElement.removeChild(singleElement);    
    }

    addLoader(resultContainer);

    const id = parseInt(seriesId.replace('#', ''), 10);
    if(isNaN(id)) {
        cards.insertAdjacentHTML('afterend', `
        <section class="single">
            <div class="container">
                <div class="alert alert-red"><i class="fa fa-exclamation-circle"></i> Invalid ID, Not A Number '${seriesId}' </div>
            </div>
        </section>
        `);
    }
    else {
        // - GET TV SHOW INFO
        const seriesDetails = await getTvSeriesDetailsById(id);
        console.log(seriesDetails);
        console.log(`seriesDetails.status: ${seriesDetails.status}`);
        if (seriesDetails.status === 404) {
            cards.insertAdjacentHTML('afterend', `
            <section class="single">
                <div class="container">
                    <div class="alert alert-red"><i class="fa fa-exclamation-circle"></i> No TV Show Found With The ID '${id}' </div>
                </div>
            </section>
            `);
        }
        else {
            // - RENDER PAGE
            let image = seriesDetails.image /* el.show.image !== null */
                ? `<img src="${seriesDetails.image.medium}" alt="Show Image" />`
                : '<img src="images/noImg.png" alt="Show Image" />';

            let genres = '';
            seriesDetails.genres.forEach((genre) => genres += `<span class="pill">${genre}</span>`);
            if (!genres) {
                genres = '<span class="pill red-pill">None</span>';
            }
            
            let rate = seriesDetails.rating.average ? `<p class="lang"><span class="t"><i class="fa fa-star"></i> Rating:</span> <strong>${seriesDetails.rating.average}</strong> / 10</p>` : '';
            let link = seriesDetails.officialSite ? `<div class="links"><a class="more" href="${seriesDetails.officialSite}" target="_blanc" >Official Site</a></div>` : '';
            
            const page = `
            <section class="single">
                <div class="container">
                    <div class="left">
                        <a id="backlink" class="back" href="TvSeries.html"><i class="fa fa-arrow-left"></i> Back</a>
                        ${image}
                    </div>
                    <div class="right">
                        <h1>${seriesDetails.name}</h1>
                        ${seriesDetails.summary}
                        <span class="t"><i class="fa fa-theater-masks"></i> Genres: </span>
                        <div class="genres-pills">
                            ${genres}
                        </div>
                        <p class="lang"><span class="t"><i class="fa fa-language"></i> Language:</span> ${seriesDetails.language}</p>
                        ${rate}
                        ${link}
                    </div>
                </div>
            </section><br>`;

            cards.insertAdjacentHTML('afterend', page);
            
            document.getElementById('backlink').addEventListener('click', e => {
                e.preventDefault();
                prevQuery = "";

                if (searchForm['search'].value) {                    
                    searchForm['btn'].click();
                } else {
                    if (seriesDetails.name) {
                        let arr = seriesDetails.name.split(' ');
                        searchForm['search'].value = arr[Math.floor(Math.random() * (arr.length - 1))];
                        searchForm['btn'].click();
                    } else {
                        window.location = document.getElementById('backlink').href;
                    }
                }
            })            
        }            
    }

    removeLoader();
}

let prevQuery = "";
searchForm.addEventListener('submit', async (e) => {
    e.preventDefault();

    const query = searchForm['search'].value;    
    if (query !== "" && query !== prevQuery) {
        prevQuery = query;

        history.replaceState(null, null, ' ');
        cards.style.display = "block";
        resultContainer.innerHTML = '';

        const singleElement = document.querySelector('.single'); 
        if(singleElement !== null) {
            singleElement.parentElement.removeChild(singleElement);    
        }

        addLoader(resultContainer);

        const seriesFound = await getTvSeriesBySearchQuery(query);

        let HTML = '';
        seriesFound.forEach(el => {
            let image = el.show.image /* el.show.image !== null */
                ? `<img src="${el.show.image.medium}" alt="Show Image" />`
                : '<img src="images/noImg.png" alt="Show Image" />';

            let genres = '';
            el.show.genres.forEach((genre) => genres += `<span class="pill">${genre}</span>`);
            if (!genres) {
                genres = '<span class="pill red-pill">None</span>';
            }

            HTML += `
            <a class="show" href="#${el.show.id}">
            <div class="card">
                ${image}
                <div class="content">
                    <p class="name">${el.show.name}</p>
                    <div class="genres">
                        ${genres}
                    </div>
                </div>
            </div>
                <h4>${el.show.name}</h4>
            </a>
        `;
        });

        if (!HTML) {
            HTML = `<div class="alert alert-red"><i class="fa fa-exclamation-circle"></i> No Result Found For '${query}'</div>`;
        }

        resultContainer.insertAdjacentHTML('afterbegin', HTML);

        removeLoader();
    }
});

const addLoader = (parent) => {
    parent.insertAdjacentHTML('afterbegin', '<div class="loader" id="loader"><i class="fa fa-undo spin fa-4x"></i></div>');
}
const removeLoader = () => document.getElementById('loader').parentElement.removeChild(document.getElementById('loader'));

/* Example request.
https://api.tvmaze.com/search/shows?q=vikings
[{"score":26.522043,"show":{"id":29,"url":"http://www.tvmaze.com/shows/29/vikings","name":"Vikings","type":"Scripted","language":"English","genres":["Drama","Action","History"],"status":"Running","runtime":60,"premiered":"2013-03-03","officialSite":"https://www.history.ca/shows/vikings/","schedule":{"time":"21:00","days":["Friday"]},"rating":{"average":8.7},"weight":100,"network":{"id":118,"name":"History","country":{"name":"Canada","code":"CA","timezone":"America/Halifax"}},"webChannel":{"id":1,"name":"Netflix","country":null},"externals":{"tvrage":31136,"thetvdb":260449,"imdb":"tt2306299"},"image":{"medium":"http://static.tvmaze.com/uploads/images/medium_portrait/286/715906.jpg","original":"http://static.tvmaze.com/uploads/images/original_untouched/286/715906.jpg"},"summary":"<p><b>Vikings</b> transports us to the brutal and mysterious world of Ragnar Lothbrok, a Viking warrior and farmer who yearns to explore - and raid - the distant shores across the ocean.</p>","updated":1610543234,"_links":{"self":{"href":"http://api.tvmaze.com/shows/29"},"previousepisode":{"href":"http://api.tvmaze.com/episodes/1993638"},"nextepisode":{"href":"http://api.tvmaze.com/episodes/1993639"}}}},{"score":24.049517,"show":{"id":1335,"url":"http://www.tvmaze.com/shows/1335/vikings","name":"Vikings","type":"Documentary","language":"English","genres":["Drama","Adventure","Fantasy"],"status":"Ended","runtime":60,"premiered":"2012-09-11","officialSite":"http://www.bbc.co.uk/programmes/b01ms4sh","schedule":{"time":"21:00","days":["Tuesday"]},"rating":{"average":8.5},"weight":70,"network":{"id":37,"name":"BBC Two","country":{"name":"United Kingdom","code":"GB","timezone":"Europe/London"}},"webChannel":null,"externals":{"tvrage":32540,"thetvdb":262368,"imdb":"tt2455488"},"image":{"medium":"http://static.tvmaze.com/uploads/images/medium_portrait/7/18620.jpg","original":"http://static.tvmaze.com/uploads/images/original_untouched/7/18620.jpg"},"summary":"<p>Neil Oliver goes in search of the truth about the <b>Vikings</b> beyond their violent reputation.</p>","updated":1581449931,"_links":{"self":{"href":"http://api.tvmaze.com/shows/1335"},"previousepisode":{"href":"http://api.tvmaze.com/episodes/111678"}}}},{"score":19.994892,"show":{"id":45121,"url":"http://www.tvmaze.com/shows/45121/vikings-valhalla","name":"Vikings: Valhalla","type":"Scripted","language":"English","genres":["Drama","Action","History"],"status":"In Development","runtime":60,"premiered":null,"officialSite":"https://www.netflix.com/title/81149450","schedule":{"time":"","days":[]},"rating":{"average":null},"weight":85,"network":null,"webChannel":{"id":1,"name":"Netflix","country":null},"externals":{"tvrage":null,"thetvdb":372700,"imdb":"tt11311302"},"image":null,"summary":"<p>Continuing the storytelling of <i>Vikings</i>, the new saga begins 100 years after the original series concludes and dramatizes the adventures of the most famous Vikings who ever lived - Leif Erikson, Freydis, Harald Harada and the Norman King William the Conqueror (also a Viking descendant). These men and women will blaze new paths as they fight for survival in an ever-changing and evolving Europe. This is the explosive next chapter of the Vikings legend.</p>","updated":1602801866,"_links":{"self":{"href":"http://api.tvmaze.com/shows/45121"}}}},{"score":15.553811,"show":{"id":16214,"url":"http://www.tvmaze.com/shows/16214/vikings-athelstans-journal","name":"Vikings: Athelstan's Journal","type":"Scripted","language":"English","genres":["Drama","Action","History"],"status":"Ended","runtime":5,"premiered":"2015-01-30","officialSite":null,"schedule":{"time":"","days":[]},"rating":{"average":null},"weight":48,"network":null,"webChannel":{"id":94,"name":"History Channel","country":{"name":"United States","code":"US","timezone":"America/New_York"}},"externals":{"tvrage":null,"thetvdb":null,"imdb":"tt4622118"},"image":{"medium":"http://static.tvmaze.com/uploads/images/medium_portrait/54/135411.jpg","original":"http://static.tvmaze.com/uploads/images/original_untouched/54/135411.jpg"},"summary":"<p>Torn between two faiths, Athelstan pores over his journal looking for answers about where his heart belongs in this \"Vikings\" webisode.</p>","updated":1503979707,"_links":{"self":{"href":"http://api.tvmaze.com/shows/16214"},"previousepisode":{"href":"http://api.tvmaze.com/episodes/738877"}}}},{"score":13.74988,"show":{"id":22980,"url":"http://www.tvmaze.com/shows/22980/real-vikings","name":"Real Vikings","type":"Documentary","language":"English","genres":["Drama","History"],"status":"Ended","runtime":60,"premiered":"2016-11-30","officialSite":"http://www.history.ca/real-vikings/","schedule":{"time":"22:00","days":["Wednesday"]},"rating":{"average":7.8},"weight":50,"network":{"id":118,"name":"History","country":{"name":"Canada","code":"CA","timezone":"America/Halifax"}},"webChannel":null,"externals":{"tvrage":null,"thetvdb":320625,"imdb":"tt5470756"},"image":{"medium":"http://static.tvmaze.com/uploads/images/medium_portrait/91/228824.jpg","original":"http://static.tvmaze.com/uploads/images/original_untouched/91/228824.jpg"},"summary":"<p><b>Real Vikings</b> dives deep into the history and archaeology that inspires the popular drama Vikings. From \"test-driving\" Viking longships to exploring the legendary roots of Ragnar Lothbrok, this companion series gets the scoop on Vikings from some of the world's top Viking experts. Joining them at key Viking sites are actors from the drama series: Clive Standen, pictured above (Rollo), Maude Hirst (Helga), Alyssa Sutherland (Aslaug) and Katheryn Winnick (Lagertha). By combining compelling footage from the dramatic series with the best in documentary filmmaking, viewers will get a never-before-seen view of what Vikings were really like.</p>","updated":1574377785,"_links":{"self":{"href":"http://api.tvmaze.com/shows/22980"},"previousepisode":{"href":"http://api.tvmaze.com/episodes/998286"}}}},{"score":10.826105,"show":{"id":35892,"url":"http://www.tvmaze.com/shows/35892/tales-of-the-vikings","name":"Tales of the Vikings","type":"Scripted","language":"English","genres":["Action","Adventure"],"status":"Ended","runtime":30,"premiered":"1959-09-08","officialSite":null,"schedule":{"time":"","days":[]},"rating":{"average":null},"weight":0,"network":{"id":72,"name":"Syndication","country":{"name":"United States","code":"US","timezone":"America/New_York"}},"webChannel":null,"externals":{"tvrage":null,"thetvdb":70547,"imdb":"tt0053543"},"image":null,"summary":"<p>Set in tenth century Scandinavia, the series presented the seafaring exploits of Viking chief Firebeard and his two sons, Leif and Finn.</p>","updated":1523151072,"_links":{"self":{"href":"http://api.tvmaze.com/shows/35892"},"previousepisode":{"href":"http://api.tvmaze.com/episodes/1441938"}}}},{"score":10.826105,"show":{"id":40938,"url":"http://www.tvmaze.com/shows/40938/americas-lost-vikings","name":"America's Lost Vikings","type":"Documentary","language":"English","genres":["History"],"status":"Ended","runtime":60,"premiered":"2019-02-10","officialSite":"https://www.sciencechannel.com/tv-shows/americas-lost-vikings/","schedule":{"time":"22:00","days":["Sunday"]},"rating":{"average":null},"weight":29,"network":{"id":77,"name":"Science","country":{"name":"United States","code":"US","timezone":"America/New_York"}},"webChannel":null,"externals":{"tvrage":null,"thetvdb":359471,"imdb":"tt9680558"},"image":{"medium":"http://static.tvmaze.com/uploads/images/medium_portrait/205/513184.jpg","original":"http://static.tvmaze.com/uploads/images/original_untouched/205/513184.jpg"},"summary":"<p>The Vikings are amongst the most feared warriors and ruthless raiders in history. They are also at the center of an incredible mystery. There is evidence that Norseman landed on the coast of North America 500 years before Christopher Columbus. Beyond this, however, the trail goes cold. Now archaeologists and explorers Blue Nelson and Mike Arbuthnot will embark on their biggest ever adventure – to find out how far the Vikings explored into America.</p><p>To solve this mystery, Blue and Mike will use a combination of state-of-the-art science as well as gripping hands-on experimental archaeology. Their journey is documented in the <b>Americas Lost Vikings</b>.</p>","updated":1586111911,"_links":{"self":{"href":"http://api.tvmaze.com/shows/40938"},"previousepisode":{"href":"http://api.tvmaze.com/episodes/1622200"}}}},{"score":10.826105,"show":{"id":27858,"url":"http://www.tvmaze.com/shows/27858/blood-of-the-vikings","name":"Blood of the Vikings","type":"Documentary","language":"English","genres":["History"],"status":"Ended","runtime":60,"premiered":"2001-11-05","officialSite":null,"schedule":{"time":"","days":["Monday"]},"rating":{"average":null},"weight":16,"network":{"id":12,"name":"BBC One","country":{"name":"United Kingdom","code":"GB","timezone":"Europe/London"}},"webChannel":null,"externals":{"tvrage":null,"thetvdb":79358,"imdb":"tt0410965"},"image":{"medium":"http://static.tvmaze.com/uploads/images/medium_portrait/110/275734.jpg","original":"http://static.tvmaze.com/uploads/images/original_untouched/110/275734.jpg"},"summary":"<p>Julian Richards examines remains that are the earliest archaeological evidence of the infamous Viking hit-and-run raids in Britain. Loot from our fair Isles has been found hidden in Viking graves in Norway giving evidence that they did manage to take some of Britain home with them.</p>","updated":1581299190,"_links":{"self":{"href":"http://api.tvmaze.com/shows/27858"},"previousepisode":{"href":"http://api.tvmaze.com/episodes/1162345"}}}},{"score":7.7916436,"show":{"id":24619,"url":"http://www.tvmaze.com/shows/24619/greeks-romans-vikings-the-founders-of-europe","name":"Greeks, Romans, Vikings: The Founders of Europe","type":"Documentary","language":"English","genres":["History"],"status":"Ended","runtime":60,"premiered":"2014-10-19","officialSite":null,"schedule":{"time":"19:30","days":["Sunday"]},"rating":{"average":null},"weight":0,"network":{"id":140,"name":"SBS","country":{"name":"Australia","code":"AU","timezone":"Australia/Sydney"}},"webChannel":null,"externals":{"tvrage":null,"thetvdb":287443,"imdb":null},"image":{"medium":"http://static.tvmaze.com/uploads/images/medium_portrait/94/235143.jpg","original":"http://static.tvmaze.com/uploads/images/original_untouched/94/235143.jpg"},"summary":"<p>What were the Vikings really like? What did the Romans accomplish? And do the singular achievements of the ancient Greeks live on in our society today? This series questions the myths and unravels age-old clichés about these ancient cultures. It examines their strange and sometimes amusing idiosyncrasies, gives fresh insights into who they really were, and provides a novel take on their societies, peppered with surprising new revelations. Democracy, science, theatre, plumbing, modern roads, laws, the Olympics, conquering the seas, and the onset of globalisation are all at the heart of modern society, but in fact were inherited from the ancient worlds of the Greeks, the Romans and the Vikings. This is a new look at the ancient civilisations that will change our perception of them.</p>","updated":1484913309,"_links":{"self":{"href":"http://api.tvmaze.com/shows/24619"},"previousepisode":{"href":"http://api.tvmaze.com/episodes/1045121"}}}},{"score":5.5004807,"show":{"id":20974,"url":"http://www.tvmaze.com/shows/20974/vikingane","name":"Vikingane","type":"Scripted","language":"Norwegian","genres":["Comedy","Action","History"],"status":"Ended","runtime":30,"premiered":"2016-10-14","officialSite":"https://tv.nrk.no/serie/vikingane","schedule":{"time":"22:25","days":["Friday"]},"rating":{"average":7.6},"weight":94,"network":null,"webChannel":{"id":238,"name":"NRK TV","country":{"name":"Norway","code":"NO","timezone":"Europe/Oslo"}},"externals":{"tvrage":null,"thetvdb":318009,"imdb":"tt5905354"},"image":{"medium":"http://static.tvmaze.com/uploads/images/medium_portrait/238/596752.jpg","original":"http://static.tvmaze.com/uploads/images/original_untouched/238/596752.jpg"},"summary":"<p>Set in 790AD, <b>Vikingane</b> features the daily challenges of people living in a small Viking village, from power struggle, brother rivalry, gender equality, to betrayal and friendship. \"It's the story of people from our time, but living during the Viking era. Of course everyday choices have far more dramatic consequences and that makes for great comedy material.</p>","updated":1609186474,"_links":{"self":{"href":"http://api.tvmaze.com/shows/20974"},"previousepisode":{"href":"http://api.tvmaze.com/episodes/1795957"}}}}]
*/
const getTvSeriesBySearchQuery = async (query) => {
    console.log(`${API}/search/shows?q=${query}`);
    const data = await fetch(`${API}/search/shows?q=${query}`)
        .then(res => res.json());

    return data;
}

/* Example request.
https://api.tvmaze.com/shows/20974
{"id":20974,"url":"http://www.tvmaze.com/shows/20974/vikingane","name":"Vikingane","type":"Scripted","language":"Norwegian","genres":["Comedy","Action","History"],"status":"Ended","runtime":30,"premiered":"2016-10-14","officialSite":"https://tv.nrk.no/serie/vikingane","schedule":{"time":"22:25","days":["Friday"]},"rating":{"average":7.6},"weight":94,"network":null,"webChannel":{"id":238,"name":"NRK TV","country":{"name":"Norway","code":"NO","timezone":"Europe/Oslo"}},"externals":{"tvrage":null,"thetvdb":318009,"imdb":"tt5905354"},"image":{"medium":"http://static.tvmaze.com/uploads/images/medium_portrait/238/596752.jpg","original":"http://static.tvmaze.com/uploads/images/original_untouched/238/596752.jpg"},"summary":"<p>Set in 790AD, <b>Vikingane</b> features the daily challenges of people living in a small Viking village, from power struggle, brother rivalry, gender equality, to betrayal and friendship. \"It's the story of people from our time, but living during the Viking era. Of course everyday choices have far more dramatic consequences and that makes for great comedy material.</p>","updated":1609186474,"_links":{"self":{"href":"http://api.tvmaze.com/shows/20974"},"previousepisode":{"href":"http://api.tvmaze.com/episodes/1795957"}}}
*/
const getTvSeriesDetailsById = async (id) => {
    console.log(`${API}/shows/${id}`);
    const data = await fetch(`${API}/shows/${id}`)
        .then(res => res.json());

    return data;
}