
function fetchData() {
    const url ="data/phones.json";

    fetch(url)
    .then((res) => res.json())
    .then((data) => {
        updateWebPage(data);
    })
    .catch((error) => {
        console.error('Error:', error);
    });
}

function updateWebPage(phones) {
    // logo and header
    // var header = document.createElement("header");
    // var img = document.createElement("img");
    // img.setAttribute("src","logo.png");
    // img.setAttribute("alt","logo");
    // img.setAttribute("height","40px");
    // img.setAttribute("width","40px");
    // header.append(img);
    // var Content = document.createTextNode(" Apple Store"); 
    // header.appendChild(Content); 
    // document.body.appendChild(header);

    // section for the products
    let section = document.createElement("section");
    section.classList.add("products");
    document.body.appendChild(section);

    // for loop used to loop all the products
    for(let i = 0; i < phones.length; i++) {
        //console.log(phones[i]);
        let item = document.createElement("div");
        item.classList.add("item");
        section.append(item);
   
        let img = document.createElement("img");
        img.setAttribute("src","images/" + phones[i].image);
        img.setAttribute("alt",phones[i].name);
        item.append(img);

        let details = document.createElement("div");
        details.classList.add("details");
        item.append(details);

        // phone modal
        let title = document.createElement("p"); 
        let Content = document.createTextNode(phones[i].name); 
        title.classList.add("title");
        title.appendChild(Content); 
        details.append(title);

        // phone price
        let price = document.createElement("p"); 
        let priceContent = document.createTextNode(phones[i].price); 
        price.classList.add("price");
        price.appendChild(priceContent); 
        details.append(price);

        // description about the phone in list format       
        let ul = document.createElement('ul');
       details.appendChild(ul);

       phones[i].list.forEach(function(name){
           let li = document.createElement('li');
           ul.appendChild(li);
           let i = document.createElement('i');
           i.classList.add("fa");
           i.classList.add("fa-check");
           li.append(i);
           li.innerHTML += name;
       });
       
       // buy button 
       let button = document.createElement("button");
       button.innerHTML = "BUY NOW";
       details.append(button);
    }    
}

//Entry point
(function() {
    fetchData();
})();

