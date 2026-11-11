const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const vm = require('node:vm');

test('initialization configures error capture once and queues .NET exceptions until the SDK loads', () => {
    const scripts = [];
    const window = { location: { pathname: '/portal' } };
    const context = vm.createContext({
        window,
        document: {
            cookie: '',
            createElement: () => ({ setAttribute() {} }),
            getElementsByTagName: () => [{ parentNode: { appendChild: script => scripts.push(script) } }]
        },
        navigator: { userAgent: 'Test browser' },
        setTimeout: callback => callback(),
        URL
    });
    const source = fs.readFileSync(path.join(__dirname,
        '../../src/Soenneker.Blazor.ApplicationInsights/wwwroot/applicationinsightsinterop.js'), 'utf8');
    vm.runInContext(source.replaceAll('export function', 'function'), context);
    context.trackError('Before init');
    context.init('InstrumentationKey=test');
    context.init('InstrumentationKey=test');
    assert.equal(scripts.length, 1);
    assert.equal(window.appInsights.config.disableExceptionTracking, false);
    assert.equal(window.appInsights.config.enableUnhandledPromiseRejectionTracking, true);
    assert.equal(typeof window.onerror, 'function');
    context.trackError('Render failed', 'System.InvalidOperationException: failure\n at Render()',
        'System.InvalidOperationException', 3, 'Renderer', 100, 'RenderFailure');
    let captured;
    window.appInsights.trackPageView = () => {};
    window.appInsights.trackException = telemetry => { captured = telemetry; };
    for (const queued of window.appInsights.queue) queued();
    assert.equal(captured.exception.message, 'Render failed');
    assert.equal(captured.exception.name, 'System.InvalidOperationException');
    assert.equal(captured.exception.stack, 'System.InvalidOperationException: failure\n at Render()');
    assert.equal(captured.severityLevel, 3);
    assert.equal(captured.properties.category, 'Renderer');
    assert.equal(captured.properties.eventId, 100);
});
