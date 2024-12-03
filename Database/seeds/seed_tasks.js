/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('TASK').del();

  // Dynamic username
  const username = 'myAccount123'; // Change this to a variable as needed

  // Define a constant for NullDateTime
  const NullDateTime = new Date(1999, 0, 1, 1, 1, 1).toISOString();

  // Insert seed entries
  await knex('TASK').insert([
      { 
          username: username, 
          name: "Workout", 
          due_date: new Date(Date.now() + 18000000).toISOString(), // DateTime.Now.AddHours(5)
          description: "", // No description
          completed: false, 
          important: false, 
          repeat_option: "Daily", 
          reminder: new Date(Date.now() + 14400000).toISOString(), // DateTime.Now.AddHours(4)
          note_id: null 
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
          note_id: null 
      },
      { 
          username: username, 
          name: "Prepare Presentation", 
          due_date: new Date(Date.now() + 172800000).toISOString(), // DateTime.Now.AddDays(2)
          description: "For client meeting", 
          completed: false, 
          important: true, 
          repeat_option: "", 
          reminder: new Date(Date.now() + 151200000).toISOString(), // DateTime.Now.AddDays(1).AddHours(18)
          note_id: null 
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
          note_id: null 
      },
      { 
          username: username, 
          name: "Clean House", 
          due_date: new Date(Date.now() + 345600000).toISOString(), // DateTime.Now.AddDays(4)
          description: "", // No description
          completed: false, 
          important: false, 
          repeat_option: "Weekly", 
          reminder: new Date(Date.now() + 276480000).toISOString(), // DateTime.Now.AddDays(3).AddHours(16)
          note_id: null 
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
          note_id: null 
      },
      { 
          username: username, 
          name: "Buy Groceries", 
          due_date: new Date(Date.now() + 86400000).toISOString(), // DateTime.Now.AddDays(1)
          description: "Milk, Bread, Eggs", 
          completed: false, 
          important: true, 
          repeat_option: "Weekly", 
          reminder: new Date(Date.now() + 82800000).toISOString(), // DateTime.Now.AddHours(23)
          note_id: null 
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
          note_id: null 
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
          note_id: null 
      },
      { 
          username: username, 
          name: "Complete Project Report", 
          due_date: new Date(Date.now() + 432000000).toISOString(), // DateTime.Now.AddDays(5)
          description: "Submit to Manager", 
          completed: false, 
          important: true, 
          repeat_option: "", 
          reminder: NullDateTime, // No reminder
          note_id: null 
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
          note_id: null 
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
          note_id: null 
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
          note_id: null 
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
          note_id: null 
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
          note_id: null 
      }
  ]);
};
