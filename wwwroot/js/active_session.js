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

// Save the current session data to Local Storage only
function cacheSessionDataToBrowserStorage() {
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

// Submit the form to the server and clear Local Storage
function endSession() {
    document.getElementById("conversationDataInput").value = JSON.stringify(conversationData);
    document.getElementById("SessionAndConversationInfoForm").submit();
    localStorage.removeItem('activeSessionData');
    console.log("End Session: Data submitted and Local Storage cleared.");
}

function showPopupMessage(message) {
    let popup = document.getElementById('popupMessage');
    if (!popup) {
        popup = document.createElement('div');
        popup.id = 'popupMessage';
        popup.style.position = 'fixed';
        popup.style.top = '20px';
        popup.style.left = '50%';
        popup.style.transform = 'translateX(-50%)';
        popup.style.backgroundColor = '#d4edda';
        popup.style.color = '#155724';
        popup.style.padding = '10px 20px';
        popup.style.borderRadius = '5px';
        popup.style.boxShadow = '0 0 10px rgba(0, 0, 0, 0.1)';
        popup.style.zIndex = '1000';
        popup.style.fontWeight = 'bold';
        popup.style.textAlign = 'center';
        popup.style.minWidth = '200px';
        document.body.appendChild(popup);
    }
    popup.innerText = message;
    popup.style.display = 'block';
    setTimeout(() => {
        popup.style.display = 'none';
    }, 3000);
}

function autoResizeTextarea(textarea) {
    textarea.style.height = 'auto';
    textarea.style.height = textarea.scrollHeight + 'px';
}

function renderConversations() {
    const tableBody = document.getElementById('conversationsTableBody');
    tableBody.innerHTML = ''; // Clear all existing rows
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
            <td>
                <div class="resizable-comment">
                    <textarea class="form-control"
                        rows="1"
                        oninput="updateConversation(${index}, 'comment', this.value); autoResizeTextarea(this);"
                    >${conversation.comment}</textarea>
                </div>
            </td>
            <td><button type="button" class="btn btn-danger" onclick="deleteRow(this, ${index})">Delete</button></td>
        `;
        tableBody.appendChild(row);

        // Auto-resize the textarea initially
        const textarea = row.querySelector('textarea');
        autoResizeTextarea(textarea);
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
    renderConversations();
}

// Function to set the formatted date on page load or date change
function initializeDate() {
    const dateInput = document.getElementById('date');
    if (!dateInput.value) {
        const today = new Date().toISOString().split('T')[0];
        dateInput.value = today;
    }
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

function updateSessionDuration() {
    const startTimeInput = document.getElementById('TimeOfADayStart');
    const endTimeInput = document.getElementById('TimeOfADayEnd');
    const durationInput = document.getElementById('SessionDuration');
    const sessionDurationForDb = document.getElementById('SessionDurationHidden');

    if (!startTimeInput.value || !endTimeInput.value) {
        durationInput.value = "";
        sessionDurationForDb.value = "";
        return;
    }

    const [startHours, startMinutes] = startTimeInput.value.split(":").map(Number);
    const [endHours, endMinutes] = endTimeInput.value.split(":").map(Number);

    const now = new Date();
    const startTime = new Date(now.getFullYear(), now.getMonth(), now.getDate(), startHours, startMinutes);
    const endTime = new Date(now.getFullYear(), now.getMonth(), now.getDate(), endHours, endMinutes);

    if (endTime < startTime) {
        endTime.setDate(endTime.getDate() + 1);
    }

    let diff = (endTime - startTime) / 60000; // Convert milliseconds to minutes
    const diffHours = Math.floor(diff / 60);
    const diffMinutes = diff % 60;

    durationInput.value = `${diffHours}h ${diffMinutes}m`;
    sessionDurationForDb.value = `${diffHours}:${diffMinutes}`; // Format for server-side binding
}

// Attach event listeners when the DOM is fully loaded
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById('TimeOfADayStart').addEventListener("change", updateSessionDuration);
    document.getElementById('TimeOfADayEnd').addEventListener("change", updateSessionDuration);
});