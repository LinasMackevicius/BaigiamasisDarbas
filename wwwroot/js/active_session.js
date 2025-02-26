﻿
let conversationData = []; // Store conversation data locally
let currentCommentInput = null;

// Load data from Local Storage on page load (for temporary saving)
window.addEventListener('load', () => {
    const savedData = JSON.parse(localStorage.getItem('activeSessionData'));
    if (savedData) {
        document.getElementById('date').value = savedData.date;
        document.getElementById('place').value = savedData.place;
        document.getElementById('goals').value = savedData.goals;
        conversationData = savedData.conversations || [];
        renderConversations();
    }
});

// Save the current session data to Local Storage when "Save Session" is clicked
function saveSession() {
    document.getElementById("conversationDataInput").value = JSON.stringify(conversationData);
    const sessionData = {
        date: document.getElementById('date').value,
        place: document.getElementById('place').value,
        goals: document.getElementById('goals').value,
        conversations: conversationData
    };

    localStorage.setItem('activeSessionData', JSON.stringify(sessionData));
    console.log("Session temporarily saved in Local Storage:", sessionData);

    showPopupMessage('Saved to browser local storage!');
}

function showPopupMessage(message) {
    const popup = document.createElement('div');
    popup.style.position = 'absolute';
    popup.style.bottom = '80%'; // Adjust this value as needed to position above the button
    popup.style.left = '50%';
    popup.style.transform = 'translateX(-50%)';
    popup.style.backgroundColor = '#d4edda';
    popup.style.color = '#155724';
    popup.style.padding = '10px 20px';
    popup.style.borderRadius = '5px';
    popup.style.boxShadow = '0 0 10px rgba(0, 0, 0, 0.1)';
    popup.style.zIndex = '1000';
    popup.style.fontWeight = 'bold';
    popup.innerText = message;
    document.body.appendChild(popup);

    setTimeout(() => {
        popup.remove();
    }, 3000);
}



// Function to send data to the server and clear Local Storage when "End Session" is clicked
function endSession() {
    // Gather current data from page fields
    document.getElementById("conversationDataInput").value = JSON.stringify(conversationData);

    document.getElementById("saveSessionForm").submit(); // This triggers the form submission

    // Clear Local Storage after submitting
    localStorage.removeItem('activeSessionData');
    console.log("End Session: Data submitted and Local Storage cleared.");
}




// Helper function to re-render the conversation list on the page
function renderConversations() {
    const tableBody = document.getElementById('conversationsTableBody');
    tableBody.innerHTML = ''; // Clear all existing rows

    // Loop over each item in conversationData and add a row for it
    conversationData.forEach((conversation, index) => {
        const row = document.createElement('tr');
        row.innerHTML = `
    <td><input type="text" class="form-control" value="${conversation.personName}" oninput="updateConversation(${index}, 'personName', this.value)"></td>
    <td><input type="number" class="form-control" value="${conversation.duration}" oninput="updateConversation(${index}, 'duration', this.value)"></td>
    <td>
        <select class="form-control" onchange="updateConversation(${index}, 'successRating', this.value)">
            <option value="1" ${conversation.successRating == 1 ? 'selected' : ''}>1 - Unsuccessful</option>
            <option value="2" ${conversation.successRating == 2 ? 'selected' : ''}>2</option>
            <option value="3" ${conversation.successRating == 3 ? 'selected' : ''}>3 - Neutral</option>
            <option value="4" ${conversation.successRating == 4 ? 'selected' : ''}>4</option>
            <option value="5" ${conversation.successRating == 5 ? 'selected' : ''}>5 - Successful</option>
        </select>
    </td>
    <td><input type="text" class="form-control" value="${conversation.comment}" oninput="updateConversation(${index}, 'comment', this.value)"></td>
    <td><button type="button" class="btn btn-danger" onclick="deleteRow(this, ${index})">Delete</button></td>
    `;
        tableBody.appendChild(row); // Append row to table
    });
}


// Function to add a new conversation row
function addConversationRow() {
    const conversation = {
        personName: '',
        duration: 0,
        successRating: 1,
        comment: ''
    };
    conversationData.push(conversation);

    // Re-render the conversation rows with the new addition
    renderConversations();
}

// Function to update conversation data when any field changes
function updateConversation(index, field, value) {
    conversationData[index][field] = value;
    document.getElementById("conversationDataInput").value = JSON.stringify(conversationData);
}

// Function to delete a conversation row and update conversation data
function deleteRow(button, index) {
    conversationData.splice(index, 1);
    renderConversations(); // Re-render the rows after deletion
}

// Function to save edited comment from modal
function saveComment() {
    if (currentCommentInput) {
        currentCommentInput.value = document.getElementById('modalCommentText').value;
    }
    var myModalEl = document.getElementById('commentModal');
    var modal = bootstrap.Modal.getInstance(myModalEl);
    modal.hide();
}


// Function to set the formatted date on page load or date change
function initializeDate() {
    // Set current date if SessionDate is empty
    const dateInput = document.getElementById('date');
    if (!dateInput.value) {
        const today = new Date().toISOString().split('T')[0];
        dateInput.value = today;
    }

    // Display formatted date
    updateFormattedDate();
}

// Function to update the formatted date field based on date picker value
function updateFormattedDate() {
    const dateInput = document.getElementById('date');
    const formattedDateInput = document.getElementById('formattedDate');
    const date = new Date(dateInput.value);
    const formattedDate = date.getFullYear() + '/' + String(date.getMonth() + 1).padStart(2, '0') + '/' + String(date.getDate()).padStart(2, '0');
    formattedDateInput.value = formattedDate;
}

// Initialize the date field on page load
window.addEventListener('load', initializeDate);

document.addEventListener("DOMContentLoaded", function () {
    const button = document.querySelector(".btn-primary");
    const menu = document.getElementById("customMenu");

    // Function to show custom menu
    button.addEventListener("contextmenu", function (event) {
        event.preventDefault(); // Prevent default right-click menu

        // Position the menu at the cursor
        menu.style.left = `${event.pageX}px`;
        menu.style.top = `${event.pageY}px`;
        menu.style.display = "block";
    });

    // Hide menu when clicking outside
    document.addEventListener("click", function (event) {
        if (!menu.contains(event.target) && event.target !== button) {
            menu.style.display = "none";
        }
    });
});


document.addEventListener("DOMContentLoaded", function () {
    // Get references to the input fields
    const startTimeInput = document.getElementById("Session_TimeOfADayStart");
    const endTimeInput = document.getElementById("Session_TimeOfADayEnd");
    const durationInput = document.getElementById("SessionDuration");

    function calculateDuration() {
        const startTime = startTimeInput.value;
        const endTime = endTimeInput.value;

        if (!startTime || !endTime) {
            durationInput.value = ""; // Clear if any field is empty
            return;
        }

        // Convert time strings to Date objects
        const start = new Date(`2000-01-01T${startTime}`);
        const end = new Date(`2000-01-01T${endTime}`);

        // Handle overnight sessions (e.g., 23:30 - 03:00)
        if (end < start) {
            end.setDate(end.getDate() + 1); // Move end time to next day
        }

        // Calculate duration in minutes
        const durationMinutes = (end - start) / (1000 * 60); // Convert ms to minutes

        // Format to HH:MM
        const hours = Math.floor(durationMinutes / 60);
        const minutes = durationMinutes % 60;
        durationInput.value = `${hours}h ${minutes}m`;
    }

    // Attach event listeners to both time inputs
    startTimeInput.addEventListener("input", calculateDuration);
    endTimeInput.addEventListener("input", calculateDuration);
});