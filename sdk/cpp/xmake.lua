add_rules("mode.debug", "mode.release")

add_requires("cxxopts >=3.2.1")
add_requires("glaze >=5.0.2")
add_requires("libhv >=1.3.3", {configs = {http_server = false}})
add_requires("magic_enum >=0.9.7")
add_requires("spdlog >=1.15.2")

target("agent")
    set_kind("binary")
    add_packages("cxxopts", "glaze", "libhv", "magic_enum", "spdlog")
    add_includedirs("src")
    add_files("src/**.cpp")
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
