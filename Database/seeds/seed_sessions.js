const fs = require('fs');
const path = require('path');

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.seed = async function(knex) {
    // Deletes ALL existing entries
    await knex('SESSION').del();

    // Read the single username from the file
    const username = fs.readFileSync(path.join(__dirname, 'username.txt'), 'utf-8').trim();

    // Helper function to get a random integer between min and max (inclusive)
    function getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    // Inserts seed entries with equal number of "Focus" and "Break" sessions for each tag
    const sessions = [];
    const tags = ['Working', 'Studying', 'Reading'];
    const numSessionsPerTag = 12; // Total number of sessions per tag

    const now = new Date();
    const oneWeekAgo = new Date();
    oneWeekAgo.setDate(now.getDate() - 7);

    tags.forEach(tag => {
        const focusSessions = [];
        const breakSessions = [];

        for (let i = 0; i < numSessionsPerTag / 2; i++) {
            const focusDuration = getRandomInt(20 * 60, 35 * 60); // Duration in seconds
            const breakDuration = getRandomInt(10 * 60, 15 * 60); // Duration in seconds

            // Generate a random date between one week ago and now
            const focusTimestamp = new Date(oneWeekAgo.getTime() + Math.random() * (now.getTime() - oneWeekAgo.getTime())).toISOString();
            const breakTimestamp = new Date(oneWeekAgo.getTime() + Math.random() * (now.getTime() - oneWeekAgo.getTime())).toISOString();

            focusSessions.push({ username, duration: focusDuration, tag, timestamp: focusTimestamp, type: 'Focus' });
            breakSessions.push({ username, duration: breakDuration, tag, timestamp: breakTimestamp, type: 'Break' });
        }

        sessions.push(...focusSessions, ...breakSessions);
    });

    await knex('SESSION').insert(sessions);
};
