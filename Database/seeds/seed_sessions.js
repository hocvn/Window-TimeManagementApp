const fs = require('fs');
const path = require('path');

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.seed = async function(knex) {
    // Deletes ALL existing entries
    await knex('FOCUS_SESSION').del();

    // Read the single username from the file
    const username = fs.readFileSync(path.join(__dirname, 'username.txt'), 'utf-8').trim();

    // Inserts seed entries with updated tags
    await knex('FOCUS_SESSION').insert([
        { username: username, duration: 3600, tag: 'Working', timestamp: '2024-12-19 10:00:00' },
        { username: username, duration: 7200, tag: 'Studying', timestamp: '2024-12-20 11:00:00' },
        { username: username, duration: 1800, tag: 'Reading', timestamp: '2024-12-21 12:00:00' },
        { username: username, duration: 5400, tag: 'Working', timestamp: '2024-12-19 10:30:00' },
        { username: username, duration: 3600, tag: 'Studying', timestamp: '2024-12-20 11:30:00' },
        { username: username, duration: 900, tag: 'Reading', timestamp: '2024-12-21 12:30:00' },
        { username: username, duration: 6000, tag: 'Working', timestamp: '2024-12-24 15:00:00' },
        { username: username, duration: 4200, tag: 'Studying', timestamp: '2024-12-25 16:00:00' },
        { username: username, duration: 3000, tag: 'Reading', timestamp: '2024-12-26 17:00:00' },
        { username: username, duration: 5000, tag: 'Working', timestamp: '2024-12-28 19:00:00' },
        { username: username, duration: 4800, tag: 'Studying', timestamp: '2024-12-30 21:00:00' },
        { username: username, duration: 5400, tag: 'Reading', timestamp: '2024-12-31 22:00:00' },
        { username: username, duration: 7500, tag: 'Working', timestamp: '2025-01-03 08:00:00' },
        { username: username, duration: 4300, tag: 'Studying', timestamp: '2025-01-04 07:00:00' },
        { username: username, duration: 3700, tag: 'Reading', timestamp: '2025-01-05 06:00:00' },
        { username: username, duration: 5400, tag: 'Working', timestamp: '2025-01-07 04:00:00' },
        { username: username, duration: 6200, tag: 'Studying', timestamp: '2025-01-08 03:00:00' },
        { username: username, duration: 5800, tag: 'Reading', timestamp: '2025-01-09 02:00:00' },
        { username: username, duration: 6300, tag: 'Working', timestamp: '2025-01-11 00:00:00' },
        { username: username, duration: 4600, tag: 'Studying', timestamp: '2025-01-13 22:00:00' },
        { username: username, duration: 5200, tag: 'Reading', timestamp: '2025-01-14 21:00:00' },
        { username: username, duration: 5900, tag: 'Reading', timestamp: '2025-01-15 20:00:00' },
        { username: username, duration: 5600, tag: 'Working', timestamp: '2025-01-16 19:00:00' },
        { username: username, duration: 6700, tag: 'Studying', timestamp: '2025-01-19 16:00:00' },
        { username: username, duration: 3700, tag: 'Reading', timestamp: '2025-01-20 15:00:00' },
        { username: username, duration: 4800, tag: 'Studying', timestamp: '2025-01-26 09:00:00' },
        { username: username, duration: 6700, tag: 'Working', timestamp: '2025-01-27 08:00:00' },
        { username: username, duration: 4900, tag: 'Reading', timestamp: '2025-01-28 07:00:00' },
        { username: username, duration: 5100, tag: 'Reading', timestamp: '2025-01-29 06:00:00' },
        { username: username, duration: 5300, tag: 'Studying', timestamp: '2025-01-30 05:00:00' },
        { username: username, duration: 5800, tag: 'Working', timestamp: '2025-01-31 04:00:00' },
        { username: username, duration: 5200, tag: 'Reading', timestamp: '2025-02-02 02:00:00' }
    ]);
};
