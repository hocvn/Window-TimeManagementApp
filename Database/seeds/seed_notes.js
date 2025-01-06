const fs = require('fs');
const path = require('path');

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.seed = async function(knex) {
    // Deletes ALL existing entries
    await knex('NOTE').del();

    // Read the single username from the file
    const username = fs.readFileSync(path.join(__dirname, 'username.txt'), 'utf-8').trim();

    // Insert seed entries for the single user with RTF formatted content
    await knex('NOTE').insert([
        { 
            username: username, 
            name: "Project Planning", 
            content: "{\\rtf1\\fbidis\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang1033{\\fonttbl{\\f0\\fnil Segoe UI Variable;}}{\\colortbl ;\\red232\\green89\\blue67;\\red255\\green255\\blue255;\\red0\\green0\\blue0;\\red107\\green191\\blue237;\\red111\\green255\\blue43;}{\\*\\generator Riched20 3.1.0006}\\viewkind4\\uc1 \\pard\\tx720\\cf1\\highlight2\\f0\\fs21 Effective project planning involves setting clear goals and objectives.\\cf3  Each project must have a defined scope and deliverables that align with the overall vision. This ensures that the team understands the project's purpose and their roles within it. Detailed planning helps in allocating resources efficiently and mitigating potential risks.\\par\\par\\cf4\\i A comprehensive project plan includes a timeline with specific milestones.\\cf3\\i0  These milestones act as checkpoints to monitor progress and ensure the project stays on track. Regular review and updates to the project plan are essential to address any deviations and incorporate changes as needed. This iterative process helps in maintaining project momentum.\\par\\par\\cf5\\ul Communication is a critical aspect of project planning.\\cf3\\ulnone  Establishing regular communication channels and meetings ensures that all stakeholders are informed and engaged. Transparency in sharing updates and challenges builds trust and fosters collaboration. Tools like project management software can streamline this process, making it easier to track tasks and responsibilities.\\par\\par\\b Finally,\\b0  project planning must include a risk management strategy. Identifying potential risks early and developing contingency plans can prevent disruptions. This proactive approach helps in maintaining project quality and meeting deadlines. A successful project plan is flexible, allowing for adjustments while keeping the end goal in sight.\\par}"
        },
        { 
            username: username, 
            name: "Grocery Shopping", 
            content: "1. Buy vegetables, fruits, and dairy products.\n2. Get household essentials.\n3. Check for ongoing discounts."
        },
        { 
            username: username, 
            name: "Workout Routine", 
            content: "Monday: Cardio and Strength.\nWednesday: HIIT and Abs."
        }
    ]);
};
