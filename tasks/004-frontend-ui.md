# Task 004 — Frontend UI

Build the web page.

## Layout

- Three buttons labelled **1**, **2**, **3**
- A single readout area that displays the returned total
- A loading state while the request is in flight
- A simple error state if the request fails

## Behaviour

- Clicking button N calls `GET /result?count=N` on the orchestrator
- Displays the `total` from the response
- Buttons are disabled while a request is in flight

## Technical notes

- Plain HTML, CSS, vanilla JS — no framework, no build step
- The orchestrator base URL should be configurable via a `const` at the top of `app.js` so it can point to localhost during development and the API Gateway URL in production
- No `<template>` element needed unless it feels natural — keep it simple

## Acceptance criteria

- Page loads without errors
- Clicking each button displays a number (or 0) in the readout
- Works against a locally running orchestrator
