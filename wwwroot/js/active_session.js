let conversationData = [];
let currentCommentInput = null;

window.addEventListener('load', () => {
    // Initialize date on first load
    initializeDate();

    // Load saved data from localStorage
    const savedData = JSON.parse(localStorage.getItem('activeSessionData'));
    if (savedData) {
        document.getElementById('date').value = savedData.date;
        document.getElementById('place').value = savedData.place;
        document.getElementById('goals').value = savedData.goals;
        conversationData = savedData.conversations || [];
        renderConversations();
    }

    // If no localStorage data, try restoring from server hidden field
    if (conversationData.length === 0) {
        const storedJson = document.getElementById('conversationDataInput').value;
        if (storedJson) {
            try {
                const conversations = JSON.parse(storedJson);
                for (const convo of conversations) {
                    addConversationRow(convo);
                }
            } catch (err) {
                console.error("Error parsing server-persisted conversation data:", err);
            }
        }
    }
});

// Save to localStorage only (does not submit to server)
function cacheSessionDataToBrowserStorage() {
    const sessionData = {
        date: document.getElementById('date').value,
        place: document.getElementById('place').value,
        goals: document.getElementById('goals').value,
        conversations: conversationData
    };
    localStorage.setItem('activeSessionData', JSON.stringify(sessionData));
    showPopupMessage('Saved to browser local storage!');
    console.log("Session temporarily saved:", sessionData);
}

// Submit the form to the server
function endSession() {
    document.getElementById("conversationDataInput").value = JSON.stringify(conversationData);
    document.getElementById("SessionAndConversationInfoForm").submit();
    localStorage.removeItem('activeSessionData');
    console.log("Form submitted. LocalStorage cleared.");
}

function addConversationRow(data = {}) {
    const conversation = {
        personName: data.personName || '',
        duration: data.duration || 0,
        successRating: data.successRating || 1,
        comment: data.comment || ''
    };
    conversationData.push(conversation);
    renderConversations();
}

function updateConversation(index, field, value) {
    conversationData[index][field] = value;
    document.getElementById("conversationDataInput").value = JSON.stringify(conversationData);
}

function deleteRow(button, index) {
    conversationData.splice(index, 1);
    renderConversations();
}

function renderConversations() {
    const tableBody = document.getElementById('conversationsTableBody');
    tableBody.innerHTML = '';

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
                <textarea class="form-control" rows="1" oninput="updateConversation(${index}, 'comment', this.value); autoResizeTextarea(this);">${conversation.comment}</textarea>
            </td>
            <td><button type="button" class="btn btn-danger" onclick="deleteRow(this, ${index})">Delete</button></td>
        `;
        tableBody.appendChild(row);

        const textarea = row.querySelector('textarea');
        autoResizeTextarea(textarea);
    });

    document.getElementById("conversationDataInput").value = JSON.stringify(conversationData);
}

function autoResizeTextarea(textarea) {
    textarea.style.height = 'auto';
    textarea.style.height = textarea.scrollHeight + 'px';
}

function initializeDate() {
    const dateInput = document.getElementById('date');
    if (!dateInput.value) {
        const today = new Date().toISOString().split('T')[0];
        dateInput.value = today;
    }
    updateFormattedDate();
}

function updateFormattedDate() {
    const dateInput = document.getElementById('date');
    const formattedDateInput = document.getElementById('formattedDate');
    const date = new Date(dateInput.value);
    const formatted = `${date.getFullYear()}/${String(date.getMonth() + 1).padStart(2, '0')}/${String(date.getDate()).padStart(2, '0')}`;
    formattedDateInput.value = formatted;
}

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
    const start = new Date(now.getFullYear(), now.getMonth(), now.getDate(), startHours, startMinutes);
    const end = new Date(now.getFullYear(), now.getMonth(), now.getDate(), endHours, endMinutes);

    if (end < start) end.setDate(end.getDate() + 1);

    const diff = (end - start) / 60000;
    const hours = Math.floor(diff / 60);
    const minutes = diff % 60;

    durationInput.value = `${hours}h ${minutes}m`;
    sessionDurationForDb.value = `${hours}:${minutes}`;
}

document.addEventListener("DOMContentLoaded", () => {
    document.getElementById('TimeOfADayStart').addEventListener("change", updateSessionDuration);
    document.getElementById('TimeOfADayEnd').addEventListener("change", updateSessionDuration);
});
