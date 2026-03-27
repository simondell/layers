# Task 004 — Frontend UI

Build the web page and host it on S3.

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
- The orchestrator base URL is a `const` at the top of `app.js` — points to localhost during development, API Gateway URL in production
- No `<template>` element needed unless it feels natural — keep it simple

## Hosting

- S3 bucket configured for static website hosting
- Bucket policy allows public `s3:GetObject`
- Files: `index.html`, `app.js`
- Terraform manages the bucket, website configuration, and bucket policy in `infra/`
- Orchestrator Lambda must return `Access-Control-Allow-Origin: *` so the browser can call it from the S3 origin

## Acceptance criteria

- `index.html` and `app.js` served from S3 static website URL
- Clicking each button displays a number (or 0) in the readout
- Works against the deployed orchestrator
