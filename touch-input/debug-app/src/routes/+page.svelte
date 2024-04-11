<script>
	import Main from '$lib/components/Main.svelte';
	import Background from './Background.svelte';
</script>

<Background></Background>
<Main></Main>

<style>
	:global(body) {
		font-family: 'Roboto', sans-serif;
		margin: 0;
		padding: 0;
		height: 100vh;
		overflow: hidden; /* Ensure the SVG doesn't cause scrolling */
	}

	:global(.card) {
		background: rgba(255, 255, 255, 0.4);
		border-radius: 16px;
		box-shadow: 0 4px 30px rgba(0, 0, 0, 0.1);
		backdrop-filter: blur(11.3px);
		-webkit-backdrop-filter: blur(11.3px);
		border: 1px solid rgba(255, 255, 255, 0.16);
		padding-top: 2rem;
		padding-bottom: 2rem;
	}
</style>
