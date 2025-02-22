﻿

function getIngredients() {
    const loader = document.getElementById('loader');
    loader.style.display = 'none';
    const endpoint = 'http://localhost:6002/api/ingredientitems/';
    fetch(endpoint)
        .then(function (response) {
            if (response.ok) {
                return response.json();
            }
            else {
                console.log('Error');
            }
        })
        .then(function (data) {
            console.log(data);
            displayIngredients(data);
        })
        .catch(function (error) {
            waitForData();
            console.error('Failed to receive ingredients from storage.', error);
        })
}

function displayIngredients(data) {
    const tbody = document.getElementById('ingredients-list');
    data.forEach(ingredient => {
        let row = tbody.insertRow();
        let name = row.insertCell(0);
        name.innerHTML = `${ingredient.ingredientName}`;
        let price = row.insertCell(1);
        price.innerHTML = `${ingredient.price}`;
    
        let inputCell = row.insertCell(2);
        let inputField = document.createElement('input');
        inputField.value = ingredient.quantity;
        inputField.type = 'number';
        inputField.id = `updated-quantity${ingredient.id}`;
        inputField.className = 'form-control';
        inputCell.appendChild(inputField);
                                      
        let quantityButton = row.insertCell(3);
        const updateQuantityButton = document.createElement('button');
       
        updateQuantityButton.className = 'update-quantity-button';
        updateQuantityButton.innerHTML = 'Update quantity';
        updateQuantityButton.type = 'submit';
        updateQuantityButton.id = `updated-quantity-button${ingredient.id}`;
 
        updateQuantityButton.addEventListener('click', function (e) {
            e.preventDefault();
            const updatedQuantity = parseInt(document.getElementById(`updated-quantity${ingredient.id}`).value);
            updateStorage(ingredient.id, updatedQuantity);
        });
        quantityButton.appendChild(updateQuantityButton);

        tbody.appendChild(row);

    });
}

function waitForData() {
    const loader = document.getElementById('loader');
    loader.style.display = 'block';
    setTimeout(function () {
        getIngredients();
    }, 5000);
}

function updateStorage(id,quantity) {
    let endpoint =`http://localhost:6002/api/ingredientitems/${id}/${quantity}`;

    fetch(endpoint, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' }
       })
        .then(response => response.json)
        .then(data => console.info(data))
        .catch(error => console.error(error));

    location.reload();
}

function massDelivery() {
    const endpoint = 'http://localhost:6002/api/ingredientitems/massdelivery';
    fetch(endpoint, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' }
    })
        .then(response => response.json)
        .then(data => console.log(data))
        .catch(error => console.error('Failed to increase ingredients quantity.', error));
    location.reload();
}


getIngredients();