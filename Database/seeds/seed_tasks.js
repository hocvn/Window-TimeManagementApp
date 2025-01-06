const fs = require('fs');
const path = require('path');

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.seed = async function(knex) {
    // Deletes ALL existing entries
    await knex('TASK').del();
  
    // Read the single username from the file
    const username = fs.readFileSync(path.join(__dirname, 'username.txt'), 'utf-8').trim();
  
    // Define a constant for NullDateTime
    const NullDateTime = new Date(1999, 0, 1, 1, 1, 1).toISOString();
  
    // Define possible statuses
    const statuses = ["Not Started", "In Progress", "Completed", "On Hold"];
  
    // Function to get a random status excluding "Completed"
    const getRandomStatus = () => {
        const nonCompletedStatuses = statuses.filter(status => status !== "Completed");
        return nonCompletedStatuses[Math.floor(Math.random() * nonCompletedStatuses.length)];
    };
  
    // Insert seed entries
    await knex('TASK').insert([
        { 
            username: username, 
            name: "Workout", 
            due_date: new Date(Date.now() - 86400000).toISOString(), // DateTime.Now.AddDays(-1)
            description: "", // No description
            completed: true, // Mark as completed
            important: false, 
            repeat_option: "Daily", 
            reminder: new Date(Date.now() - 82800000).toISOString(), // DateTime.Now.AddDays(-1).AddHours(1)
            note_id: null,
            status: "Completed"  // Set status to "Completed"
        },
        { 
            username: username, 
            name: "Book Flight Tickets", 
            due_date: new Date(Date.now() + 864000000).toISOString(), // DateTime.Now.AddDays(10)
            description: "Vacation to Bali", 
            completed: false, 
            important: false,
            repeat_option: "", 
            reminder: new Date(Date.now() + 820800000).toISOString(), // DateTime.Now.AddDays(9).AddHours(19)
            note_id: null,
            status: getRandomStatus()  // Add random status
        },
        { 
            username: username, 
            name: "Prepare Presentation", 
            due_date: new Date(Date.now() - 172800000).toISOString(), // DateTime.Now.AddDays(-2)
            description: "For client meeting", 
            completed: true, // Mark as completed
            important: true, 
            repeat_option: "", 
            reminder: new Date(Date.now() - 151200000).toISOString(), // DateTime.Now.AddDays(-1).AddHours(18)
            note_id: null,
            status: "Completed"  // Set status to "Completed"
        },
        { 
            username: username, 
            name: "Read Book", 
            due_date: new Date(Date.now() + 1209600000).toISOString(), // DateTime.Now.AddDays(14)
            description: "Finish reading 'Atomic Habits'", 
            completed: false, 
            important: false, 
            repeat_option: "", 
            reminder: NullDateTime, // No reminder
            note_id: null,
            status: getRandomStatus()  // Add random status
        },
        { 
            username: username, 
            name: "Clean House", 
            due_date: new Date(Date.now() - 345600000).toISOString(), // DateTime.Now.AddDays(-4)
            description: "", // No description
            completed: true, // Mark as completed
            important: false, 
            repeat_option: "Weekly", 
            reminder: new Date(Date.now() - 276480000).toISOString(), // DateTime.Now.AddDays(-3).AddHours(16)
            note_id: null,
            status: "Completed"  // Set status to "Completed"
        },
        { 
            username: username, 
            name: "Study for Exams", 
            due_date: new Date(Date.now() + 1296000000).toISOString(), // DateTime.Now.AddDays(15)
            description: "Math and Science", 
            completed: false, 
            important: true, 
            repeat_option: "", 
            reminder: NullDateTime, // No reminder
            note_id: null,
            status: getRandomStatus()  // Add random status
        },
        { 
            username: username, 
            name: "Buy Groceries", 
            due_date: new Date(Date.now() + 86400000).toISOString(), // DateTime.Now.AddDays(1)
            description: "", 
            completed: false, 
            important: true, 
            repeat_option: "Weekly", 
            reminder: new Date(Date.now() + 82800000).toISOString(), // DateTime.Now.AddHours(23)
            note_id: null,
            status: getRandomStatus()  // Add random status
        },
        { 
            username: username, 
            name: "Doctor Appointment", 
            due_date: new Date(Date.now() + 259200000).toISOString(), // DateTime.Now.AddDays(3)
            description: "Annual Check-up", 
            completed: false, 
            important: true, 
            repeat_option: "", 
            reminder: new Date(Date.now() + 194400000).toISOString(), // DateTime.Now.AddDays(2).AddHours(22)
            note_id: null,
            status: getRandomStatus()  // Add random status
        },
        { 
            username: username, 
            name: "Pay Bills", 
            due_date: new Date(Date.now() + 604800000).toISOString(), // DateTime.Now.AddDays(7)
            description: "Electricity and Water", 
            completed: false, 
            important: true, 
            repeat_option: "Monthly", 
            reminder: new Date(Date.now() + 518400000).toISOString(), // DateTime.Now.AddDays(6).AddHours(21)
            note_id: null,
            status: getRandomStatus()  // Add random status
        },
        { 
            username: username, 
            name: "Complete Project Report", 
            due_date: new Date(Date.now() - 432000000).toISOString(), // DateTime.Now.AddDays(-5)
            description: "Submit to Manager", 
            completed: true, // Mark as completed
            important: true, 
            repeat_option: "", 
            reminder: NullDateTime, // No reminder
            note_id: null,
            status: "Completed"  // Set status to "Completed"
        },
        { 
            username: username, 
            name: "Attend Team Meeting", 
            due_date: new Date(Date.now() + 7200000).toISOString(), // DateTime.Now.AddHours(2)
            description: "Discuss project status", 
            completed: false, 
            important: false, 
            repeat_option: "Daily", 
            reminder: NullDateTime, // No reminder
            note_id: null,
            status: getRandomStatus()  // Add random status
        },
        { 
            username: username, 
            name: "Call Mom", 
            due_date: new Date(Date.now() + 93600000).toISOString(), // DateTime.Now.AddDays(1).AddHours(1)
            description: "Catch up and check on her", 
            completed: false, 
            important: true, 
            repeat_option: "Weekly", 
            reminder: new Date(Date.now() + 82800000).toISOString(), // DateTime.Now.AddHours(23)
            note_id: null,
            status: getRandomStatus()  // Add random status
        },
        { 
            username: username, 
            name: "Dentist Appointment", 
            due_date: new Date(Date.now() + 777600000).toISOString(), // DateTime.Now.AddDays(9)
            description: "Teeth cleaning", 
            completed: false, 
            important: true, 
            repeat_option: "", 
            reminder: new Date(Date.now() + 691200000).toISOString(), // DateTime.Now.AddDays(8).AddHours(14)
            note_id: null,
            status: getRandomStatus()  // Add random status
        },
        { 
            username: username, 
            name: "Plan Birthday Party", 
            due_date: new Date(Date.now() + 1036800000).toISOString(), // DateTime.Now.AddDays(12)
            description: "Buy decorations and cake", 
            completed: false, 
            important: false,
            repeat_option: "", 
            reminder: NullDateTime, // No reminder
            note_id: null,
            status: getRandomStatus()  // Add random status
        },
        { 
            username: username, 
            name: "Meditation", 
            due_date: new Date(Date.now() + 10800000).toISOString(), // DateTime.Now.AddHours(3)
            description: "20 minutes mindfulness", 
            completed: false, 
            important: false, 
            repeat_option: "Daily", 
            reminder: NullDateTime, // No reminder
            note_id: null,
            status: getRandomStatus()  // Add random status
        }
    ]);
};