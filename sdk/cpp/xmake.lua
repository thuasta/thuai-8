add_rules("mode.debug", "mode.release")

add_requires("cxxopts 3.2.1")
add_requires("fmt 10.2.1")
add_requires("libhv 1.3.2", {configs = {http_server = false}})
add_requires("magic_enum v0.9.6")
add_requires("nlohmann_json v3.11.3")
add_requires("spdlog v1.14.1")

target("agent")
    set_kind("binary")
    add_packages("cxxopts", "fmt", "libhv", "magic_enum", "nlohmann_json", "spdlog")
    add_includedirs("src")
    add_files("src/**.cc")
    set_languages("cxx23")
    set_exceptions("cxx")
    set_warnings("allextra")
    
    if is_plat("windows") then
        add_defines("NOMINMAX")
    end

    after_build(function (target)
        os.cp(
            target:targetfile(), 
            path.join(os.projectdir(), "bin", path.filename(target:targetfile()))
        )
    end)
