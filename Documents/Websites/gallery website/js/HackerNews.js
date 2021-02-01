const container = document.getElementById('inner-container');

const API = {
    url:"https://hacker-news.firebaseio.com"
}

function fetchData() {

    const url = `${API.url}/v0/showstories.json?print=pretty`;
    const itemIDs = []; 
    console.log(url);
    fetch(url)
        .then((res) => res.json())
        .then((data) => {
            for (let i = 0; i < data.length; i++) {
                console.log(`${i}, ${data[i]}`);
                itemIDs.push(data[i]);
            }    
            getItems(itemIDs);                    
        });
}

function getItems(array){

    for (let i = 0; i < array.length; i++) {
        const url = `${API.url}/v0/item/${array[i]}.json?print=pretty`;
        console.log(url);

        fetch(url)
            .then((res) => res.json())
            .then((data) => {
                const item = {
                    "by" : data.by,
                    "descendants" : data.descendants,
                    "id" : data.id,
                    "kids" : data.kids,
                    "score" : data.score,
                    "time" : data.time,
                    "title" : data.title,
                    "type" : data.type,
                    "url" : data.url
                }

                createElement(item);
            });        
    }
}

function createElement(item){

    const storyElement = document.createElement('div');
    storyElement.classList.add('Story-Element');

    const rowElement = document.createElement('div');
    rowElement.classList.add('Story-Row');

    const voteElement = document.createElement('p');
    voteElement.classList.add('Story-Vote');
    voteElement.innerText = item.score + " Votes";

    const timeElement = document.createElement('p');
    timeElement.classList.add('Story-Time');
    const time = moment(item.time * 1000).fromNow();
    timeElement.innerText = time;

    const titleElement = document.createElement('h3');
    titleElement.classList.add('Story-Title');
    titleElement.innerText = item.title;

    const linkElement = document.createElement('a');
    linkElement.classList.add('Story-Link');
    linkElement.href = item.url;
    linkElement.target = "_blank";
    linkElement.innerText = "LINK"

    storyElement.appendChild(rowElement);
    rowElement.appendChild(voteElement);
    rowElement.appendChild(timeElement);
    storyElement.appendChild(titleElement);
    storyElement.appendChild(linkElement);

    container.appendChild(storyElement);

}



fetchData();