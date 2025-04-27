# 🎟️ Nilah's Raffle Plugin
<div align="center">
<img src="raffler.png" alt="Raffler Logo" width="512" height="512">
</div>
✨ A lightweight Dalamud plugin for Final Fantasy XIV that allows players to run in-game raffles using gil as currency. Great for venue events, giveaways, or community nights!

---

## ✨ Current Features

✅ **Toggleable Raffle System**  
Start, stop, and draw raffles easily with `/raffler on`, `/raffler off`, and `/raffler draw`.

✅ **Bonus Tickets for Early Entries**  
Configurable "BOGO" (Buy One, Get One) bonus tickets for the first players who enter.

✅ **Chat Trigger Detection**  
Watch for trigger words (like "raffle", "ticket", "join") in chat to automatically open the raffle UI.

✅ **Persistent Entry Saving**  
Raffle entries are saved locally (`raffle_entries.json`) to survive crashes and resume your session safely.

✅ **Sequential Ticket Numbers**  
Every ticket is tracked with a clean, unique number to avoid confusion.

✅ **Dynamic Ticket List Views**  
Choose between a simplified or detailed view of all current entries.

✅ **BOGO Ticket Locking**  
Bonus ticket offers lock after the first bonus is awarded to maintain fairness.

✅ **Intuitive, Lightweight UI**  
Seamless windows to configure raffles, review entries, and draw winners.

✅ **Command Handler and Help Message**  
`/raffler` commands are fully integrated with `/xlhelp` and Dalamud command systems.

✅ **Logging and Debugging**  
Key actions and errors are cleanly logged via Dalamud's plugin log viewer (`/xllog`).

---

## 🔥 Coming Soon (Work In Progress)

🔄 **Name Prefill via Targeting**  
Autofill raffle entries using your current in-game target's name.

🏆 **Starting Pot Configuration**  
Set an initial prize pool (gil) for the raffle when it begins.

💾 **Session Recovery Improvements**  
Additional backup save formats (CSV), allowing manual recovery if needed.

📝 **Editable Player Names**  
Modify names mid-session without changing ticket numbers. Bonus: mass update all occurrences.

💬 **Session History Saving**  
Save and review past raffle sessions easily for transparency or event history.

🔔 **Discord Integration**  
Push live results to a Discord webhook automatically when drawing a winner.

🔢 **Metrics Display**  
Show real-time stats like **Tickets Sold** and **Gil Collected** at the bottom of the raffle window.

⚠️ **Confirmation Prompts**  
Add confirmations before clearing active raffles or deleting data.

🛡️ **Override Mode**  
Admin tools to override BOGO restrictions, manual entry edits, or custom ticket additions.

---

## 📜 Commands

```plaintext
/raffler        - Open the main UI window
```
---
## 📸 Screenshots
![Preview](https://github.com/user-attachments/assets/dbf6c181-7d0c-4b82-9452-98989e0a252b)


---



